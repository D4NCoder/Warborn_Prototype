using UnityEngine;
using Mirror;
using System;

namespace Warborn.Ingame.Characters.Player.PlayerManagement.Statistics
{
    public class PlayerStatsController : NetworkBehaviour
    {
        #region Stats of a player
        [SyncVar(hook = nameof(OnMovementSpeedChange))][SerializeField] private float movementSpeed;
        [SyncVar][SerializeField] private int armour;
        [SyncVar][SerializeField] private int baseHealth;
        [SyncVar(hook = nameof(OnCurrentHealthChange))][SerializeField] private int currentHealth;
        [SyncVar(hook = nameof(OnAttackDamageChange))][SerializeField] private int attackDamage;

        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
        public int Armour { get { return armour; } set { armour = value; } }
        public int BaseHealth { get { return baseHealth; } set { baseHealth = value; } }
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
        #endregion

        #region Events and Handlers
        public event Action<float> onMovementSpeedChanged;
        public void OnMovementSpeedChange(float _oldMovement, float _newMovement) => onMovementSpeedChanged?.Invoke(_newMovement);
        public event Action<int, int> onCurrentHealthChange;
        public void OnCurrentHealthChange(int _oldHealth, int _newHealth) => onCurrentHealthChange?.Invoke(_newHealth, baseHealth);

        public event Action<int> onAttackDamageChange;
        public void OnAttackDamageChange(int _oldDamage, int _newDamage) => onAttackDamageChange?.Invoke(_newDamage);

        #endregion

        #region Initialization
        [Server]
        public void InitBasicStats(float _movementSpeed, int _armour, int _attackDamage, int _baseHealth)
        {

            OnMovementSpeedChange(this.movementSpeed, _movementSpeed);
            OnAttackDamageChange(this.attackDamage, _attackDamage);

            BaseHealth = _baseHealth;
            OnCurrentHealthChange(this.currentHealth, _baseHealth);

            Armour = _armour;
            //AttackDamage = _attackDamage;
        }
        #endregion

        #region Actions for stats

        #region Health
        [Server]
        public void ChangeHealth(int _newHealth)
        {
            CurrentHealth = _newHealth;
        }
        #endregion
        #endregion
    }
}

