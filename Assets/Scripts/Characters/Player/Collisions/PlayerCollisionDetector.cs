using UnityEngine;
using Mirror;
using System;

namespace Warborn.Characters.Player.Collisions
{
    public class PlayerCollisionDetector : NetworkBehaviour
    {
        #region Events
        public event Action<bool> onHitGround;
        public event Action<GameObject> onHitByWeapon;
        #endregion

        #region Collision Detection
        [Server]
        void OnCollisionEnter(Collision collision)
        {
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
                // This works on server, the server is listening for all players
            }
        }

        [Server]
        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == CollisionType.GROUND)
            {
                onHitGround?.Invoke(false);
            }

        }
        #endregion
    }
}

