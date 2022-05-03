using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerGUI.Stats;
using Warborn.Ingame.Characters.Player.PlayerManagement.Core;
using Warborn.Ingame.Characters.Player.PlayerManagement.Statistics;
using Warborn.Ingame.Characters.Player.PlayerModel.Core;
using Warborn.Ingame.Helpers;
using Warborn.Ingame.Items.Weapons.Effects.Core;
using Warborn.Ingame.Items.Weapons.Effects.EffectsDatabase;
using Warborn.Ingame.Map.Core;

namespace Warborn.Ingame.Networking
{
    public class PlayerNetworkingController : NetworkBehaviour
    {
        #region Variables and Properties

        #region Player's information
        [Header("Player's information")]
        [SerializeField] public GameObject PlayerModel;
        [SerializeField] private PlayerInformation playerInformation = null;
        [SerializeField] private PlayerEffectsController playerEffectsController = null;
        [SerializeField] private PlayerStatsController playerStatsController = null;
        [SerializeField] private TeamNetworkingControlller teamNetworkingControlller = null;
        #endregion

        #region References
        [Header("References")]
        public GameObject PlayerPrefab;
        public GameObject PlayerGUI;
        [SerializeField] private Transform PlayerModelParent;
        [SerializeField] private Transform PlayerGUIParent;
        [SerializeField] private GameObject playerGUI;
        public Vector3 SpawnPosition;
        #endregion

        #region Event Actions
        private event Action onPlayerModelInitialized;
        #endregion

        #region Getters
        public PlayerEffectsController GetEffectsController { get { return playerEffectsController; } }
        #endregion
        #endregion

        #region Start methods
        void Start()
        {
            if (!isLocalPlayer) { return; }
            if (isClient)
            {
                CmdInitializePlayerModel();
                onPlayerModelInitialized += InitClientEvents;
            }
        }

        public override void OnStartClient()
        {
            if (!isLocalPlayer) { return; }
            PlayerGUIParent = GameObject.Find("GUI_Placeholder").transform;
            playerGUI = Instantiate(PlayerGUI, PlayerGUI.transform.position, Quaternion.identity, PlayerGUIParent);
            InitGUIChangeEvents();
        }

        public override void OnStartServer()
        {
            InitializePlayer();
            InitServerEvents();
            // TODO: Change stats based on a class that player chooses
            playerStatsController.InitBasicStats(7f, 10, 10, 200);
        }
        #endregion

        #region Initializing player
        [Server]
        private void InitializePlayer()
        {
            // TODO: Player chooses team, or gets decided before the game starts, meaning in the LOBBY
            teamNetworkingControlller = GameObject.FindWithTag(TagsReferences.TEAM1_MANAGER).GetComponent<TeamNetworkingControlller>();
            PlayerModelParent = GameObject.FindGameObjectWithTag(TagsReferences.PLAYERS_PLACEHOLDER).transform;

            playerInformation.PlayerName = "Player" + NetworkServer.connections.Count;

            PlayerModel = GameObject.Instantiate(PlayerPrefab, SpawnPosition, Quaternion.identity, PlayerModelParent);
            PlayerModel.GetComponent<PlayerController>().PlayerName = playerInformation.PlayerName;
            NetworkServer.Spawn(PlayerModel, connectionToClient);
        }

        [Command]
        public void CmdInitializePlayerModel()
        {
            if (PlayerModel == null) { return; }
            RpcInitializePlayerModel(PlayerModel.GetComponent<NetworkIdentity>());
        }


        [ClientRpc]
        public void RpcInitializePlayerModel(NetworkIdentity _player)
        {
            PlayerModel = _player.gameObject;
            onPlayerModelInitialized?.Invoke();
        }
        #endregion

        #region Events
        [Server]
        private void InitServerEvents()
        {
            PlayerModel.GetComponent<PlayerController>().PlayerCollisionDetector.onHitByWeapon += HandleHitByWeapon;
            PlayerModel.GetComponent<PlayerController>().PlayerCollisionDetector.onInteraction += HandleOnInteractionCollision;
        }

        [Client]
        private void InitClientEvents()
        {
            playerStatsController.onMovementSpeedChanged += PlayerModel.GetComponent<PlayerController>().PlayerMover.OnMovementSpeedChange;
            InitInputEvents();
        }

        [Client]
        private void InitGUIChangeEvents()
        {
            playerStatsController.onCurrentHealthChange += playerGUI.GetComponent<GUIStatsController>().OnCurrentHealthChange;

        }

        private void InitInputEvents()
        {
            PlayerModel.GetComponent<PlayerController>().InputHandler.onInteract += HandlePlayersInteraction;
        }


        #endregion

        #region Handlers

        #region Weapon hit
        [Server]
        private void HandleHitByWeapon(List<int> _effectIds)
        {
            foreach (int _effectId in _effectIds)
            {
                Effect _effect = EffectsDatabase.GetInstance().GetEffectById(_effectId);
                playerEffectsController.effects.Add(_effect);
            }
            playerEffectsController.OnEffectsApply(this.gameObject);
        }
        #endregion

        #region Interaction with interactable objects
        #region Client
        [Client]
        private void HandlePlayersInteraction()
        {
            // Player has pressed button
            CmdHandlePlayerInteraction();
        }

        [ClientRpc]
        public void RpcHandlePlayerInteraction(NetworkIdentity _interactableObject, bool _value)
        {
            _interactableObject.GetComponent<InteractableObject>().Interact(_value);
        }
        #endregion
        #region Server
        [Server]
        private void HandleOnInteractionCollision(NetworkIdentity _interactableObject, bool _value)
        {
            if (_value == true)
            {
                InteractableObject _iObject = _interactableObject.GetComponent<InteractableObject>();
                if (_iObject.isActive)
                {
                    ShowInteractionText(connectionToClient, "To deactivate " + _iObject.Name + " press X!");
                }
                else
                {
                    ShowInteractionText(connectionToClient, "To activate " + _iObject.Name + " press X!");
                }

                playerInformation.InteractingObject = _interactableObject;
            }
            else
            {
                playerInformation.InteractingObject = null;
                HideInteractionText(connectionToClient);
            }
        }

        [Command]
        public void CmdHandlePlayerInteraction()
        {
            if (playerInformation.InteractingObject == null) { return; }
            HideInteractionText(connectionToClient);
            if (playerInformation.InteractingObject == teamNetworkingControlller.ForestManager.ForestPillar)
            {
                teamNetworkingControlller.ForestManager.IsForestPillarActive = !teamNetworkingControlller.ForestManager.IsForestPillarActive;
            }
            RpcHandlePlayerInteraction(playerInformation.InteractingObject, teamNetworkingControlller.ForestManager.IsForestPillarActive);
        }
        #endregion
        #endregion

        #region Modal popup upon colliding with interactable
        // Move this to Interaction class
        [TargetRpc]
        public void ShowInteractionText(NetworkConnection _connection, string _text)
        {
            playerGUI.GetComponent<GUIStatsController>().ShowInteractionText(_text);
        }

        [TargetRpc]
        private void HideInteractionText(NetworkConnection _connection)
        {
            playerGUI.GetComponent<GUIStatsController>().HideInteractionText();
        }
        #endregion

        #endregion












    }
}

