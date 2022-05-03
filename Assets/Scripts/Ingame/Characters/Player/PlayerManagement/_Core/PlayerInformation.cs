using Mirror;
using UnityEngine;

namespace Warborn.Ingame.Characters.Player.PlayerManagement.Core
{

    public class PlayerInformation : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SetName))][SerializeField] public string playerName;
        public string PlayerName { get { return playerName; } set { playerName = value; } }

        // Identity of an object which the player can interact with 
        [SyncVar][SerializeField] private NetworkIdentity interactingObject;
        public NetworkIdentity InteractingObject { get { return interactingObject; } set { interactingObject = value; } }


        public void SetName(string _oldName, string _newName)
        {
            this.gameObject.name = _newName;
        }
    }
}

