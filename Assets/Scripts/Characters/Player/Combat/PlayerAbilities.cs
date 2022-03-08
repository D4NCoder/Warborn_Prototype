using System;
using UnityEngine;
using Warborn_Prototype.Inputs;

namespace Warborn.Characters.Player.Combat
{
    public class PlayerAbilities : MonoBehaviour
    {
        #region Controls
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

        private void InitControlsCallbacks()
        {
            // Controls.Player.Move.performed += ctx => StartMoving(ctx.ReadValue<Vector2>());
            // Controls.Player.Move.canceled += ctx => CancelMovement();
            // Controls.Player.Jump.performed += ctx => SetJump();
            // Controls.Player.Ability1.performed += ctx => UseAbility(PlayerAbilitiesEnum.Ability1);
            // Controls.Player.Ability1.performed += ctx => UseAbility(PlayerAbilitiesEnum.Ability2);
            // Controls.Player.Ability1.performed += ctx => UseAbility(PlayerAbilitiesEnum.Ability3);
        }
        #endregion

        [SerializeField] private bool isUsingAbility = false;


    }
}

