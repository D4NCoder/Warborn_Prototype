using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbornNetworkingManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
