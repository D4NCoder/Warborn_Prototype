using UnityEngine;
using Mirror;
using System;
using Warborn.Items.Weapons.Weapons.WeaponCollision;
using System.Collections.Generic;

namespace Warborn.Characters.Player.PlayerModel.Collisions
{
    public class PlayerCollisionDetector : NetworkBehaviour
    {
        #region Events
        public event Action<bool> onHitGround;
        public event Action<List<int>> onHitByWeapon;
        #endregion

        #region Collision Detection
        [Server]
        void OnCollisionEnter(Collision collision)
        {
            if (!hasAuthority) { return; }

            if (collision.gameObject.layer == CollisionType.GROUND)
            {
                onHitGround?.Invoke(true);
            }
        }

        [Server]
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == CollisionType.WEAPON)
            {
                WeaponInstanceInfo weaponInfo = other.gameObject.GetComponent<WeaponInstanceInfo>();
                if (weaponInfo.effectsToApplyToPlayer != null)
                    onHitByWeapon?.Invoke(weaponInfo.effectsToApplyToPlayer);
            }
        }

        [Server]
        void OnCollisionExit(Collision collision)
        {
            if (!hasAuthority) { return; }

            if (collision.gameObject.layer == CollisionType.GROUND)
            {
                onHitGround?.Invoke(false);
            }
        }
        #endregion
    }
}

