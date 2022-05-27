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
using Warborn.Ingame.Items.Weapons.Weapons.WeaponCollision;
using Warborn.Ingame.Map.Core;
using Warborn.Ingame.Map.Core.DamagableObjects;
using Warborn.Ingame.Settings;

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
        [SerializeField] private EntityTeamType belongingTeam;
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
        public EntityTeamType BelongingTeam { get { return belongingTeam; } }
        #endregion
        #endregion

        #region Start methods
        void Start()
        {
            if (!isLocalPlayer) { return; }
            if (isClient)
            {
                CursorSettings.LockCursor();
                CmdInitializePlayerModel();
                onPlayerModelInitialized += InitClientEvents;
            }
            if (isServer)
            {
                playerStatsController.InitBasicStats(7f, 10, 10, 200);
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
            PlayerController _playerController = PlayerModel.GetComponent<PlayerController>();
            _playerController.PlayerName = playerInformation.PlayerName;
            _playerController.PlayerNetworkManager = this.gameObject.GetComponent<NetworkIdentity>();

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
            teamNetworkingControlller = GameObject.FindWithTag(TagsReferences.TEAM1_MANAGER).GetComponent<TeamNetworkingControlller>();
            onPlayerModelInitialized?.Invoke();
        }
        #endregion

        #region Events
        [Server]
        private void InitServerEvents()
        {
            PlayerController playerController = PlayerModel.GetComponent<PlayerController>();

            playerController.PlayerCollisionDetector.onHitByWeapon += HandleHitByWeapon;
            playerController.PlayerCollisionDetector.onInteraction += HandleOnInteractionCollision;

            playerController.PlayerAbilities.onWeaponEquiped += HandleEquipedWeapon;


            // GUI
            playerController.PlayerCollisionDetector.onDamagableInteraction += HandleOnDamagableCollisionGUI;
            playerController.PlayerCollisionDetector.onDamagableLeave += HandleOnDamagableLeaveGUI;

            // Stats
            playerStatsController.onAttackDamageChange += playerController.PlayerAbilities.OnAttackDamageChange;
        }

        [Client]
        private void InitClientEvents()
        {

            // Stats
            playerStatsController.onMovementSpeedChanged += PlayerModel.GetComponent<PlayerController>().PlayerMover.OnMovementSpeedChange;

            // Input
            PlayerModel.GetComponent<PlayerController>().InputHandler.onInteract += HandlePlayersInteraction;
            playerGUI.GetComponent<GUIStatsController>().FountainOfUndyingGUI.onSpawnArmy += teamNetworkingControlller.HandleOnSpawnArmyRequest;
        }

        [Client]
        private void InitGUIChangeEvents()
        {
            playerStatsController.onCurrentHealthChange += playerGUI.GetComponent<GUIStatsController>().OnCurrentHealthChange;
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

        #region Weapon Equiped
        [Server]
        private void HandleEquipedWeapon(WeaponInstanceInfo _instanceInfo)
        {
            _instanceInfo.BelongingTeam = BelongingTeam;
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

        [TargetRpc]
        public void RpcHandlePlayerInteractionWithGUI(NetworkConnection _connection, NetworkIdentity _interactableObject)
        {
            _interactableObject.GetComponent<InteractableObject>().Interact(playerGUI);
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

            if (playerInformation.InteractingObject == teamNetworkingControlller.NormalForestManager.ForestPillar)
            {
                teamNetworkingControlller.NormalForestManager.IsForestPillarActive = !teamNetworkingControlller.NormalForestManager.IsForestPillarActive;
                RpcHandlePlayerInteraction(playerInformation.InteractingObject, teamNetworkingControlller.NormalForestManager.IsForestPillarActive);
            }
            if (playerInformation.InteractingObject == teamNetworkingControlller.DarkForestManager.ForestPillar)
            {
                teamNetworkingControlller.DarkForestManager.IsForestPillarActive = !teamNetworkingControlller.DarkForestManager.IsForestPillarActive;
                RpcHandlePlayerInteraction(playerInformation.InteractingObject, teamNetworkingControlller.DarkForestManager.IsForestPillarActive);
            }
            if (playerInformation.InteractingObject == teamNetworkingControlller.FountainOfUndying)
            {
                RpcHandlePlayerInteractionWithGUI(connectionToClient, playerInformation.InteractingObject);
            }

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

        #region Interaction with Damagables

        [Server]
        public void HandleOnDamagableCollisionGUI(NetworkIdentity _identity, EntityTeamType _team, int _maxHealth, int _currentHealth, string _name)
        {
            bool _enemyDamagable = true;
            if (belongingTeam == _team) { _enemyDamagable = false; }

            _identity.GetComponent<DamagableObject>().onHealthChange += HandleChangeOfDamagableHealth;

            RpcShowDamagableGUI(connectionToClient, _enemyDamagable, _maxHealth, _currentHealth, _name);
        }
        [Server]
        public void HandleOnDamagableLeaveGUI()
        {
            HideDamagableGUI(connectionToClient);
        }

        [Server]
        public void HandleChangeOfDamagableHealth(int _newHealth)
        {
            UpdateDamagableGUI(connectionToClient, _newHealth);
        }

        [TargetRpc]
        public void RpcShowDamagableGUI(NetworkConnection _connection, bool _enemy, int _maxHealth, int _currentHealth, string _name)
        {
            playerGUI.GetComponent<GUIStatsController>().ShowDamagableGUI(_enemy, _maxHealth, _currentHealth, _name);
        }

        [TargetRpc]
        public void UpdateDamagableGUI(NetworkConnection _connection, int _newHealth)
        {
            playerGUI.GetComponent<GUIStatsController>().UpdateHealthOfStatue(_newHealth);
        }

        [TargetRpc]
        public void HideDamagableGUI(NetworkConnection _connection)
        {
            playerGUI.GetComponent<GUIStatsController>().HideDamagableGUI();
        }
        #endregion
        #endregion

    }
}

