using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Warborn.Characters.Player.Statistics
{
    public class StatsController : MonoBehaviour
    {
        #region Statistics
        [SerializeField] private HealthController healthController;
        [SerializeField] private EffectsController effectsController;

        [SerializeField] private float movementSpeed;
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

        [SerializeField] private float basicAttack;
        public float BasicAttack { get => basicAttack; set => basicAttack = value; }

        [SerializeField] private int maxHealth;
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        #endregion



        public HealthController GetHealthController()
        {
            return healthController;
        }

        public EffectsController GetEffectsController()
        {
            return effectsController;
        }
    }
}


