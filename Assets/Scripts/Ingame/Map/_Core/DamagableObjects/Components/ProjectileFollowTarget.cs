using Mirror;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;

namespace Warborn.Ingame.Map.Core.DamagableObjects.Components
{
    [RequireComponent(typeof(NetworkTransform))]
    public class ProjectileFollowTarget : NetworkBehaviour
    {
        [SerializeField] private Transform target;
        public Transform Target { get { return target; } set { target = value; } }

        [SerializeField] private float speed;

        // TODO: Add effect

        [ServerCallback]
        private void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
        }

        [Server]
        private void OnTriggerEnter(Collider _other)
        {
            Debug.Log("The sphere has collided with player");
            if (_other.gameObject.layer == CollisionType.PLAYER)
            {
                NetworkServer.Destroy(this.gameObject);
            }
        }


    }
}


