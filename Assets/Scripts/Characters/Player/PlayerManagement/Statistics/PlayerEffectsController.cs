using UnityEngine;
using Mirror;

namespace Warborn.Characters.Player.PlayerManagement.Statistics
{
    public class PlayerEffectsController : NetworkBehaviour
    {
        [Server]
        public void OnEffectsApply(GameObject player1)
        {

        }
    }

}
