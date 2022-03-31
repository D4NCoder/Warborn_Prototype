using Mirror;
using UnityEngine;
using Warborn.Characters.Player.PlayerModel.Collisions;
using Warborn.Characters.Player.PlayerModel.Combat;
using Warborn.Characters.Player.PlayerModel.Movement;
using Warborn.Items.Weapons.Weapons.WeaponsDatabase;

namespace Warborn.Characters.Player.PlayerModel.Core
{
    public class PlayerController : NetworkBehaviour
    {
        #region Editor variables
        [Header("Player's information")]
        [SyncVar(hook = nameof(SetName))] public string PlayerName;

        [Header("Player's input")]
        [SerializeField] private InputHandler inputHandler = null;

        [Header("Player Movement")]
        [SerializeField] private PlayerMover playerMover = null;
        [SerializeField] private PlayerCollisionDetector collisionDetector = null;

        [Header("Player Combat")]
        [SerializeField] private PlayerAbilities playerAbilities = null;
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
            LockCursor();

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
            playerAbilities.UpdateWeaponsAbilities();
        }
        #endregion

        #region Handlers
        private bool HandleMovement()
        {
            return playerMover.TryMove();
        }
        private static void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

        #region Getters and Setters
        public PlayerCollisionDetector GetPlayerCollisionDetector()
        {
            return collisionDetector;
        }

        public PlayerMover GetPlayerMover()
        {
            return playerMover;
        }

        #endregion
    }
}
