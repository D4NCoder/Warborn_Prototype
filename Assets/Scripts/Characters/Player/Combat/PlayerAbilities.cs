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
            Controls.Player.BasicAttack.performed += ctx => BasicAttack();
            Controls.Player.Ability1.performed += ctx => Ability1();
            Controls.Player.Ability2.performed += ctx => Ability2();
            Controls.Player.Ability3.performed += ctx => UltimateAbility();
        }

        private void BasicAttack() => basicAttackUsed = true;

        private void Ability1() => ability1Used = true;
        private void Ability2() => ability2Used = true;
        private void UltimateAbility() => ultimateAbilityUsed = true;
        #endregion

        [Header("References")]
        [SerializeField] private Weapon equipedWeapon = null;
        [SerializeField] private GameObject localPlayer = null;
        [SerializeField] private Transform weaponPlaceholder = null;

        private bool basicAttackUsed = false;
        private bool ability1Used = false;
        private bool ability2Used = false;
        private bool ultimateAbilityUsed = false;

        private void Start()
        {
            InitControlsCallbacks();
            equipedWeapon.EquipWeapon(weaponPlaceholder);
        }


        public void UpdatePlayerAbilities()
        {
            UseAbilitiesByInput();

            if (equipedWeapon == null) { return; }

            equipedWeapon.UpdateWeapon();
        }

        private void UseAbilitiesByInput()
        {
            if (basicAttackUsed)
            {
                equipedWeapon.UseAbility(PlayerAbilityStates.BasicAttack, localPlayer);
                basicAttackUsed = false;
            }
            else if (ability1Used)
            {
                equipedWeapon.UseAbility(PlayerAbilityStates.Ability1, localPlayer);
                ability1Used = false;
            }
            else if (ability2Used)
            {
                equipedWeapon.UseAbility(PlayerAbilityStates.Ability2, localPlayer);
                ability2Used = false;

            }
            else if (ultimateAbilityUsed)
            {
                equipedWeapon.UseAbility(PlayerAbilityStates.Ultimate, localPlayer);
                ultimateAbilityUsed = false;
            }
        }

        #region Getters and Setters
        public Weapon GetEquipedWeapon()
        {
            return equipedWeapon;
        }
        #endregion
    }

}

