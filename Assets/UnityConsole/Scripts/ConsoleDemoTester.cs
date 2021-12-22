#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Collections;
using UnityEngine;

namespace CorePlugin.Samples.Scripts.Demo
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
