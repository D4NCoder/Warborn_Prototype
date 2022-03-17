using System;
using UnityEngine;

namespace Warborn.Characters.Player.Statistics
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int health = 100;

        public int GetHealth()
        {
            return health;
        }

        public void RemoveHealth(int _damage)
        {
            health -= _damage;
        }

        public void SetBaseHealth(int _health)
        {
            health = _health;
        }
    }
}

