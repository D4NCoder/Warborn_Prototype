using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warborn.Characters.Player.Core;

namespace Warborn.Networking
{
    public class PlayerNetworkingController : NetworkBehaviour
    {
        [Header("References")]
        public GameObject PlayerPrefab;
        [SerializeField] private Transform PlayerModelParent;
        public Vector3 SpawnPosition;
        [Header("Instances")]
        [SerializeField] public GameObject Player;

        void Start()
        {
            if (!isServer) { return; }
            PlayerModelParent = GameObject.FindGameObjectWithTag("PlayersPlaceholder").transform;
            Player = GameObject.Instantiate(PlayerPrefab, SpawnPosition, Quaternion.identity, PlayerModelParent);
            Player.name = "PlayerModel " + NetworkServer.connections.Count;
            NetworkServer.Spawn(Player, connectionToClient);
        }



    }
}

