using System.Collections;
using Mirror;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;
using Warborn.Ingame.Characters.Player.PlayerModel.Core;
using Warborn.Ingame.Map.Core.DamagableObjects.Components;
using Warborn.Ingame.Networking;

namespace Warborn.Ingame.Map.Core.DamagableObjects
{
    public class DamagableInteract : NetworkBehaviour
    {
        // This is just a reference for the collider
        public NetworkIdentity InteractingDamagableObject;

        [Header("Can cause damage")]
        [SerializeField] private bool canCauseDamage;
        [SerializeField] private float repeatDamageNumber;

        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectileStartingPos;

        private Transform playerPosition;

        [Server]
        private void OnTriggerEnter(Collider _other)
        {
            if (!canCauseDamage) { return; }
            if (_other.gameObject.layer == CollisionType.PLAYER)
            {
                EntityTeamType _playerTeam = _other.gameObject.GetComponent<PlayerController>().PlayerNetworkManager.GetComponent<PlayerNetworkingController>().BelongingTeam;
                EntityTeamType _statueTeam = InteractingDamagableObject.GetComponent<DamagableObject>().BelongingTeam;
                if (_playerTeam == _statueTeam) { return; }

                playerPosition = _other.gameObject.transform;
                InvokeRepeating(nameof(SpawnProjectile), 0.5f, repeatDamageNumber);
            }
        }

        [Server]
        private void OnTriggerExit(Collider _other)
        {
            if (canCauseDamage)
            {
                if (_other.gameObject.layer == CollisionType.PLAYER)
                {
                    CancelInvoke();
                }
            }
        }

        [Server]
        private void SpawnProjectile()
        {
            GameObject _projectile = Instantiate(projectilePrefab, projectileStartingPos.position, Quaternion.identity, projectileStartingPos);
            _projectile.TryGetComponent<ProjectileFollowTarget>(out ProjectileFollowTarget _projectilesTarget);
            _projectilesTarget.Target = playerPosition;
            NetworkServer.Spawn(_projectile);
        }
    }
}

