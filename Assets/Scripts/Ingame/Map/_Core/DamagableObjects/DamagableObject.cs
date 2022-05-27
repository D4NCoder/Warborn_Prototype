using UnityEngine;
using Mirror;
using System;

namespace Warborn.Ingame.Map.Core.DamagableObjects
{
    public abstract class DamagableObject : NetworkBehaviour
    {
        #region Variables
        #region References and information
        [Header("References and information")]
        [SerializeField] private Sprite icon;
        public Sprite Icon { get { return icon; } }
        [SerializeField] private string objectName;
        public string Name { get { return objectName; } }
        [SerializeField] private EntityTeamType belongingTeam;
        public EntityTeamType BelongingTeam { get { return belongingTeam; } }
        #endregion
        #region Health information
        [Header("Health information")]
        [SyncVar]
        [SerializeField] private int health;
        public int CurrentHealth { get { return health; } }
        [SyncVar]
        [SerializeField] private int maxHealth;
        public int MaxHealth { get { return maxHealth; } }
        #endregion
        #region Damage information
        [Header("Damage information")]
        [SyncVar]
        [SerializeField] private int damage;
        [SyncVar]
        [SerializeField] private bool targetable;
        #endregion
        #endregion

        public event Action<int> onHealthChange;

        public abstract void DoDamage();

        [Server]
        protected void RecieveDamage(int _damage)
        {
            this.health -= _damage;
            onHealthChange?.Invoke(this.health);
        }
    }
}

