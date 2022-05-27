using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warborn.Ingame.Helpers
{
    public class CoroutineHandler : MonoBehaviour
    {
        public Action Method;
        private IEnumerator CoroutineInvoke(Action _method, float _wait)
        {
            yield return new WaitForSeconds(_wait);
            _method?.Invoke();
        }

        public void StartCoroutineInvoke(float _wait)
        {
            //StartCoroutine(CoroutineInvoke(_method, _wait));

            Invoke(nameof(InitiateMethod), _wait);
        }

        public void InitiateMethod()
        {
            Method?.Invoke();
        }

    }

}
