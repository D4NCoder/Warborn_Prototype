using UnityEngine;
using Mirror;
using Warborn.Ingame.Map.Forest;

namespace Warborn.Ingame.Networking
{
    public class TeamNetworkingControlller : NetworkBehaviour
    {
        [SerializeField] private ForestManager forestManager;

        #region Properties
        public ForestManager ForestManager { get { return forestManager; } }

        #endregion
    }
}

