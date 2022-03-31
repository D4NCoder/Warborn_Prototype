using Mirror;

namespace Warborn.Characters.Player.PlayerManagement.Core
{
    public class PlayerInformation : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SetName))]
        public string PlayerName;

        public void SetName(string oldName, string newName)
        {
            this.gameObject.name = newName;
        }
    }
}

