using Mirror;
using UnityEngine;
using Warborn.Characters.Player.Collisions;
using Warborn.Characters.Player.Combat;
using Warborn.Characters.Player.Movement;
using Warborn.Networking;

namespace Warborn.Characters.Player.Core
{
    public class PlayerController : NetworkBehaviour
    {
        #region Editor variables
        [Header("Player's input")]
        [SerializeField] private InputHandler inputHandler = null;

        [Header("Player Movement")]
        [SerializeField] private PlayerMover playerMover = null;
        [SerializeField] private PlayerCollisionDetector collisionDetector = null;

        [Header("Player Combat")]
        [SerializeField] private PlayerAbilities playerAbilities = null;
        [SerializeField] private PlayerEffectsController playerEffects = null;
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
            if (playerAbilities.hasEquipedWeapon)
            {
                if (playerAbilities.GetEquipedWeapon() == null)
                {
                    playerAbilities.ReequipWeaponOnClientFromServer();
                }
            }
            if (hasAuthority)
            {
                this.gameObject.name = "LocalPlayer";
                InitClientEvents();
                playerAbilities.EquipWeapon();
            }
            else
            {
                this.gameObject.name = "OtherPlayer";
            }
        }

        void FixedUpdate()
        {
            if (!hasAuthority) { return; }
            if (isClient)
            {
                HandleMovement();
            }
        }

        void Update()
        {
            if (isServer)
            {
                playerAbilities.UpdateWeaponsAbilities();
            }
        }
        #endregion

        #region Handlers
        private bool HandleMovement()
        {
            return playerMover.TryMove();
        }
        #endregion

        #region Initialize events
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
            collisionDetector.onHitByWeapon += playerEffects.OnEffectsApply;
        }

        #endregion
    }
}
