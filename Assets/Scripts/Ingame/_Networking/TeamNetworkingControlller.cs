using UnityEngine;
using Mirror;
using Warborn.Ingame.Map.Forest;
using Warborn.Ingame.Map.Core;
using System.Collections.Generic;

namespace Warborn.Ingame.Networking
{
    public class TeamNetworkingControlller : NetworkBehaviour
    {
        [SerializeField] private GameObject minionPrefab;
        [SerializeField] private List<Transform> minionSpawnerPositions;
        [SerializeField] private Transform minionsParent;

        [SerializeField] private ForestManager normalForestManager;
        [SerializeField] private ForestManager darkForestManager;

        [SerializeField] private EntityTeamType belongingTeam;
        public NetworkIdentity FountainOfUndying;

        #region Properties
        public ForestManager NormalForestManager { get { return normalForestManager; } }
        public ForestManager DarkForestManager { get { return darkForestManager; } }

        public EntityTeamType BelongingTeam { get { return belongingTeam; } }
        #endregion

        #region Handlers
        [Client]
        public void HandleOnSpawnArmyRequest(int _amountOfArmy)
        {
            CmdSpawnArmy(_amountOfArmy);
        }

        [Command(requiresAuthority = false)]
        public void CmdSpawnArmy(int _amountOfArmy)
        {
            for (int i = 0; i < _amountOfArmy; i++)
            {
                GameObject _minion = Instantiate(minionPrefab, minionSpawnerPositions[i].transform.position, Quaternion.identity, minionsParent);
                NetworkServer.Spawn(_minion);
            }
        }
        #endregion
    }
}

