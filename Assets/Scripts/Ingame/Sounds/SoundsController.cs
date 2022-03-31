using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warborn.Sounds
{
    public class SoundsController : MonoBehaviour
    {
        #region Singleton
        static SoundsController instance;

        void Start()
        {
            if (instance == null) { instance = this; }
        }

        public SoundsController GetSoundsController()
        {
            return instance;
        }
        #endregion


    }
}

