using UnityEngine;
using Mirror;
using System;

namespace Warborn.Characters.Player.PlayerManagement.Statistics
{
    public class PlayerStatsController : NetworkBehaviour
    {
        public event Action<float> onMovementSpeedChanged;

        [SyncVar(hook = nameof(OnMovementSpeedChange))][SerializeField] private float movementSpeed = 100f;
        [SyncVar][SerializeField] private int armour = 10;
        [SyncVar][SerializeField] private int baseHealth = 200;
        [SyncVar][SerializeField] private int currentHealth = 200;
        [SyncVar][SerializeField] private int attackDamage = 10;

        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
        public int Armour { get { return armour; } set { armour = value; } }
        public int BaseHealth { get { return baseHealth; } set { baseHealth = value; } }
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }


        public void OnMovementSpeedChange(float _oldMovement, float _newMovement)
        {
            onMovementSpeedChanged?.Invoke(_newMovement);
        }

        public void InitBasicStats(float _movementSpeed, int _armour, int _attackDamage, int _baseHealth)
        {
            MovementSpeed = _movementSpeed;
            Armour = _armour;
            AttackDamage = _attackDamage;
            BaseHealth = _baseHealth;
            CurrentHealth = BaseHealth;
        }


    }
}

