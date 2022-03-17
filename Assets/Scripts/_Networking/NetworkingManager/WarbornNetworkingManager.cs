using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbornNetworkingManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
        player.name = "Player" + (numPlayers + 1);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
