using Mirror;
using UnityEngine;
using Warborn.Networking.Player;

namespace Warborn.Networking.Manager
{
    public class WarbornNetworkingManager : NetworkManager
    {
        [SerializeField] private Transform playersParent = null;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // Spawn Networking player prefab
            GameObject player = (GameObject)Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity, playersParent);
            // Initialize start position for model prefab, that the player will then control
            player.GetComponent<PlayerNetworkingController>().SpawnPosition = GetStartPosition().position;
            // Name the player on the server
            player.name = "Player" + (numPlayers + 1);
            // Add player to the game
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
