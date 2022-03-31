using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Warborn.Characters.Player.PlayerManagement.Core;
using Warborn.Characters.Player.PlayerManagement.Statistics;
using Warborn.Characters.Player.PlayerModel.Core;
using Warborn.Items.Weapons.Effects.Core;
using Warborn.Items.Weapons.Effects.EffectsDatabase;

namespace Warborn.Networking.Player
{
    public class PlayerNetworkingController : NetworkBehaviour
    {
        [Header("Player's information")]
        [SerializeField] public GameObject PlayerModel;
        [SerializeField] private PlayerInformation playerInformation = null;
        [SerializeField] private PlayerEffectsController playerEffectsController = null;
        [SerializeField] private PlayerStatsController playerStatsController = null;

        [Header("References")]
        public GameObject PlayerPrefab;
        public GameObject PlayerGUI;
        [SerializeField] private Transform PlayerModelParent;
        [SerializeField] private Transform PlayerGUIParent;
        public Vector3 SpawnPosition;

        void Start()
        {
            if (isServer)
            {
                InitializePlayer();
                InitServerEvents();
                playerStatsController.InitBasicStats(7f, 10, 10, 200);
            }

            if (isClient)
            {
                if (!hasAuthority) { return; }
                CmdGetPlayerModel();
                InitClientEvents();

                PlayerGUIParent = GameObject.Find("GUI_Placeholder").transform;
                Instantiate(PlayerGUI, PlayerGUI.transform.position, Quaternion.identity, PlayerGUIParent);
            }
        }


        #region Initializing player
        [Server]
        private void InitializePlayer()
        {
            playerInformation.PlayerName = "Player" + NetworkServer.connections.Count;

            PlayerModelParent = GameObject.FindGameObjectWithTag("PlayersPlaceholder").transform;
            PlayerModel = GameObject.Instantiate(PlayerPrefab, SpawnPosition, Quaternion.identity, PlayerModelParent);
            PlayerModel.GetComponent<PlayerController>().PlayerName = playerInformation.PlayerName;
            NetworkServer.Spawn(PlayerModel, connectionToClient);
        }

        [Command]
        public void CmdGetPlayerModel()
        {
            if (PlayerModel == null) { return; }

            RpcInitializePlayerModel(PlayerModel.GetComponent<NetworkIdentity>());
        }

        [ClientRpc]
        public void RpcInitializePlayerModel(NetworkIdentity _player)
        {
            PlayerModel = _player.gameObject;
        }


        #endregion


        #region Getters and Setters
        public PlayerEffectsController GetEffectsController()
        {
            return playerEffectsController;
        }
        #endregion

        #region Events
        [Server]
        private void InitServerEvents()
        {
            PlayerModel.GetComponent<PlayerController>().GetPlayerCollisionDetector().onHitByWeapon += HandleHitByWeapon;
        }

        [Client]
        private void InitClientEvents()
        {
            playerStatsController.onMovementSpeedChanged += PlayerModel.GetComponent<PlayerController>().GetPlayerMover().OnMovementSpeedChange;
        }

        [Server]
        private void HandleHitByWeapon(List<int> effectIds)
        {
            Debug.Log(PlayerModel.name + " was hit and now are applying these effects..");
            foreach (int effectId in effectIds)
            {
                Effect effect = EffectsDatabase.GetInstance().GetEffectById(effectId);
                playerEffectsController.effects.Add(effect);
            }

            playerEffectsController.OnEffectsApply(this.gameObject);
        }
        #endregion
    }
}

