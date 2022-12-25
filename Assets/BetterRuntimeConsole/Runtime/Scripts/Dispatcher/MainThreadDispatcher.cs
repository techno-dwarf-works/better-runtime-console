using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Better.RuntimeConsole.Runtime.Dispatcher
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;

        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();
        private static readonly SemaphoreSlim ExecutionQueueLock = new SemaphoreSlim(1, 1);

        public static bool IsInitialised => _instance != null;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            ExecutionQueueLock.Wait();

            try
            {
                while (ExecutionQueue.Count > 0) ExecutionQueue.Dequeue().Invoke();
            }
            finally
            {
                ExecutionQueueLock.Release();
            }
        }

        private static IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }

        private static void CreateInstance(out MainThreadDispatcher instance)
        {
            instance = new GameObject(nameof(MainThreadDispatcher)).AddComponent<MainThreadDispatcher>();
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public static void Enqueue(IEnumerator action)
        {
            GetInstance().EnqueueInternal(action);
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public static void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        public static Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Enqueue(ActionWrapper(WrappedAction));
            return tcs.Task;
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void EnqueueInternal(IEnumerator action)
        {
            ExecutionQueueLock.Wait();

            try
            {
                ExecutionQueue.Enqueue(() => { StartCoroutine(action); });
            }
            finally
            {
                ExecutionQueueLock.Release();
            }
        }

        private static MainThreadDispatcher GetInstance()
        {
            if (IsInitialised)
            {
                return _instance;
            }

            CreateInstance(out _instance);
            return _instance;
        }

        private static void Initialize()
        {
            if (IsInitialised)
            {
                return;
            }

            CreateInstance(out _instance);
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy: " + typeof(MainThreadDispatcher));
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
