using UnityEngine;
using Mirror;
using System;
using Warborn.Ingame.Items.Weapons.Weapons.WeaponCollision;
using System.Collections.Generic;

namespace Warborn.Ingame.Characters.Player.PlayerModel.Collisions
{
    public class PlayerCollisionDetector : NetworkBehaviour
    {
        #region Events
        public event Action<bool> onHitGround;
        public event Action<List<int>> onHitByWeapon;
        public event Action<NetworkIdentity, bool> onInteraction;
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
            if (_other.gameObject.layer == CollisionType.WEAPON)
            {
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
        }
        #endregion
        #endregion
    }
}

