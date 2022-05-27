using Mirror;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;
using Warborn.Ingame.Characters.Player.PlayerModel.Combat;
using Warborn.Ingame.Characters.Player.PlayerModel.Movement;
using Warborn.Ingame.Settings;
using Warborn.Items.Weapons.Weapons.WeaponsDatabase;

namespace Warborn.Ingame.Characters.Player.PlayerModel.Core
{
    public class PlayerController : NetworkBehaviour
    {
        #region Variables and Properties
        #region Players's information
        [Header("Player's information")]
        [SyncVar(hook = nameof(SetName))] public string PlayerName;

        [SyncVar]
        public NetworkIdentity PlayerNetworkManager;
        #endregion

        #region References
        [Header("Player's input")]
        [SerializeField] private InputHandler inputHandler = null;

        [Header("Player Movement")]
        [SerializeField] private PlayerMover playerMover = null;
        [SerializeField] private PlayerCollisionDetector collisionDetector = null;

        [Header("Player Combat")]
        [SerializeField] private PlayerAbilities playerAbilities = null;

        #endregion

        #region Getters
        public PlayerCollisionDetector PlayerCollisionDetector { get { return collisionDetector; } }
        public PlayerMover PlayerMover { get { return playerMover; } }
        public InputHandler InputHandler { get { return inputHandler; } }
        public PlayerAbilities PlayerAbilities { get { return playerAbilities; } }
        #endregion
        #endregion

        #region Start & Update methods
        public override void OnStartServer()
        {
            base.OnStartServer();
            InitServerEvents();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            playerAbilities.InitWeaponIfNull();

            if (!hasAuthority) { return; }
            InitClientEvents();
            playerAbilities.EquipWeapon(WeaponInstances.LIGHTNING_SWORD);
        }

        void FixedUpdate()
        {
            if (!hasAuthority) { return; }
            if (!isClient) { return; }
            HandleMovement();
        }

        void Update()
        {
            if (!isServer) { return; }
            if (!CursorSettings.IsCursorLocked()) { return; }
            playerAbilities.UpdateWeaponsAbilities();
        }
        #endregion

        #region Handlers
        private bool HandleMovement()
        {
            return playerMover.TryMove();
        }


        #endregion

        #region Initialize events
        [Client]
        private void InitClientEvents()
        {
            // Movement
            inputHandler.onStartMoving += playerMover.StartMoving;
            inputHandler.onStopMoving += playerMover.CancelMovement;
            // Combat
            inputHandler.onBasicAttack += playerAbilities.OnAbilityPressed;
            inputHandler.onAbility1 += playerAbilities.OnAbilityPressed;
            inputHandler.onAbility2 += playerAbilities.OnAbilityPressed;
            inputHandler.onUltimateAbility += playerAbilities.OnAbilityPressed;
        }

        [Server]
        private void InitServerEvents()
        {
            collisionDetector.onHitGround += playerMover.CmdOnHitGround;
        }

        private void SetName(string oldName, string newName)
        {
            this.gameObject.name = newName;
        }
        #endregion

    }
}
