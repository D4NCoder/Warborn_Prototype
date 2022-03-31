using System;
using UnityEngine;

using Mirror;
using Warborn.Characters.Player.PlayerModel.Inputs;
using Warborn.Characters.Player.PlayerModel.Combat;

/*
 * Input Handler class is purely responsible for handling input on Clients side
*/
namespace Warborn.Characters.Player.PlayerModel.Core
{
    public class InputHandler : NetworkBehaviour
    {
        #region Initializing Controls
        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }
        private void OnEnable() => Controls.Enable();
        private void OnDisable() => Controls.Disable();
        private void Start()
        {
            if (!hasAuthority)
            {
                this.enabled = false;
            }
            InitControlsCallbacks();
        }
        #endregion

        #region Event Actions
        // Movement
        public event Action<Vector2> onStartMoving;
        public event Action onStopMoving;
        // Combat
        public event Action<PlayerAbilityTypes> onBasicAttack;
        public event Action<PlayerAbilityTypes> onAbility1;
        public event Action<PlayerAbilityTypes> onAbility2;
        public event Action<PlayerAbilityTypes> onUltimateAbility;
        #endregion

        private void InitControlsCallbacks()
        {
            // Movement
            Controls.Player.Move.performed += ctx => onStartMoving?.Invoke(ctx.ReadValue<Vector2>());
            Controls.Player.Move.canceled += ctx => onStopMoving?.Invoke();
            // Combat
            Controls.Player.BasicAttack.performed += ctx => onBasicAttack?.Invoke(PlayerAbilityTypes.BasicAttack);
            Controls.Player.Ability1.performed += ctx => onAbility1?.Invoke(PlayerAbilityTypes.Ability1);
            Controls.Player.Ability2.performed += ctx => onAbility2?.Invoke(PlayerAbilityTypes.Ability2);
            Controls.Player.UltimateAbility.performed += ctx => onUltimateAbility?.Invoke(PlayerAbilityTypes.UltimateAbility);
        }
    }

}
