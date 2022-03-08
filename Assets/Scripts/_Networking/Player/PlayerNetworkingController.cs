using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkingController : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string playersPlaceholder;

    #region Server
    public override void OnStartServer()
    {
        if(playersPlaceholder != "") { ParentInit(); }

        NetworkServer.Spawn(PlayerCharacterInit(), connectionToClient);
    }

    private void ParentInit()
    {
        
        var placeHolder = GameObject.FindGameObjectWithTag(playersPlaceholder).transform;

        if(placeHolder == null) { return; }

        this.transform.SetParent(placeHolder);
    }

    private GameObject PlayerCharacterInit()
    {
        GameObject _player = GameObject.Instantiate(playerPrefab);
        _player.transform.SetParent(this.transform);

        return _player;
    }
    #endregion
}
