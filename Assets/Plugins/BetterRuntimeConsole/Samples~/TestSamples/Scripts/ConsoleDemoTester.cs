using System.Collections;
using UnityEngine;

namespace Samples
{
    public class ConsoleDemoTester : MonoBehaviour
    {
        [Min(0.1f)] [SerializeField] private float delay = 0.1f;

        private Coroutine _loggingCoroutine;

        private void Awake()
        {
            _loggingCoroutine = StartCoroutine(Logging());
        }

        private IEnumerator Logging()
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                var random = Random.Range(0, 3);

                switch (random)
                {
                    case 0:
                        Debug.Log("Simple Log");
                        break;
                    case 1:
                        Debug.LogError("Simple Log Error");
                        break;
                    case 2:
                        Debug.LogWarning("Simple Log Warning");
                        break;
                }
            }
        }

        private void OnDestroy()
        {
            StopCoroutine(_loggingCoroutine);
        }
    }
}