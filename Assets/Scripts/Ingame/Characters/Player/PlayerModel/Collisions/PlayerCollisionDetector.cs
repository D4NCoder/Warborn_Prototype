using UnityEngine;
using Mirror;
using System;
using Warborn.Ingame.Items.Weapons.Weapons.WeaponCollision;
using System.Collections.Generic;
using Warborn.Ingame.Map.Core.DamagableObjects;
using Warborn.Ingame.Map.Core;

namespace Warborn.Ingame.Characters.Player.PlayerModel.Collisions
{
    public class PlayerCollisionDetector : NetworkBehaviour
    {
        #region Events
        public event Action<bool> onHitGround;
        public event Action<List<int>> onHitByWeapon;
        public event Action<NetworkIdentity, bool> onInteraction;
        public event Action<NetworkIdentity, EntityTeamType, int, int, string> onDamagableInteraction; // NetworkId, TeamType, Max health, current health, name
        public event Action onDamagableLeave;
        #endregion

        #region Collision Detection
        #region Collisions
        [Server]
        void OnCollisionEnter(Collision _collision)
        {
            if (!hasAuthority) { return; }

            if (_collision.gameObject.layer == CollisionType.GROUND)
            {
                onHitGround?.Invoke(true);
            }
        }
        [Server]
        void OnCollisionExit(Collision _collision)
        {
            if (!hasAuthority) { return; }

            if (_collision.gameObject.layer == CollisionType.GROUND)
            {
                onHitGround?.Invoke(false);
            }
        }
        #endregion

        #region Triggers
        [Server]
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.layer == CollisionType.WEAPON || _other.gameObject.layer == CollisionType.PROJECTILE)
            {
                if (_other.transform.IsChildOf(this.gameObject.transform)) { return; }

                WeaponInstanceInfo _weaponInfo = _other.gameObject.GetComponent<WeaponInstanceInfo>();
                if (_weaponInfo.EffectsToApplyToPlayer != null)
                    onHitByWeapon?.Invoke(_weaponInfo.EffectsToApplyToPlayer);
            }
            else if (_other.gameObject.layer == CollisionType.INTERACTABLE)
            {
                if (_other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity _interactableObject) == false)
                {
                    _interactableObject = null;
                    return;
                }
                onInteraction?.Invoke(_interactableObject, true);
            }
            else if (_other.gameObject.layer == CollisionType.DAMAGABLE)
            {
                if (_other.gameObject.TryGetComponent<DamagableInteract>(out DamagableInteract _dInteract) == false) { return; }
                DamagableObject _dObject = _dInteract.InteractingDamagableObject.GetComponent<DamagableObject>();
                onDamagableInteraction?.Invoke(_dObject.GetComponent<NetworkIdentity>(), _dObject.BelongingTeam, _dObject.MaxHealth, _dObject.CurrentHealth, _dObject.Name);
            }
        }

        [Server]
        private void OnTriggerExit(Collider _other)
        {
            if (_other.gameObject.layer == CollisionType.INTERACTABLE)
            {
                if (_other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity _interactableObject) == false)
                {
                    _interactableObject = null;
                    return;
                }
                // Leaving the interactable area, thus disabling to process the interaction request
                onInteraction?.Invoke(_interactableObject, false);
            }
            if (_other.gameObject.layer == CollisionType.DAMAGABLE)
            {
                onDamagableLeave?.Invoke();
            }
        }
        #endregion
        #endregion
    }
}

