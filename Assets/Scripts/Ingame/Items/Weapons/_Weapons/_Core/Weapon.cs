using System;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Combat;
using Warborn.Ingame.Items.Weapons.Abilities.AbilitiesDatabase;
using Warborn.Ingame.Items.Weapons.Abilities.Core;

namespace Warborn.Ingame.Items.Weapons.Weapons.Core
{
    public abstract class Weapon : ICloneable
    {
        #region Class members
        public WeaponData weaponData;
        public Ability BasicAttack;
        public GameObject LocalPlayer;

        private event Action onCooldownAbilities;
        #endregion

        #region Performing the ability of a weapon
        public void PerformAbility(PlayerAbilityTypes _ability)
        {
            if (_ability == PlayerAbilityTypes.BasicAttack)
            {
                BasicAttack.PerformAbility(LocalPlayer);
                onCooldownAbilities += BasicAttack.CalculateCooldown;
                BasicAttack.onCooldownDone += OnAbilityCooldownEnded;
                BasicAttack.SetCooldown(true);
            }
        }
        public bool IsAbilityOnCooldown(PlayerAbilityTypes _type)
        {
            bool valueToReturn = false;
            if (_type == PlayerAbilityTypes.BasicAttack)
            {
                valueToReturn = BasicAttack.IsOnCooldown();
            }
            return valueToReturn;
        }
        public void CalculateAbilitiesCooldown()
        {
            if (BasicAttack == null) { return; }
            onCooldownAbilities?.Invoke();
        }

        private void OnAbilityCooldownEnded(Ability _ability)
        {
            onCooldownAbilities -= _ability.CalculateCooldown;
        }
        #endregion

        #region Initializing the weapon
        public void InitializeWeapon(GameObject _localPlayer)
        {
            LocalPlayer = _localPlayer;
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            // TODO: Go to database of Abilities and assign each and one of them
            BasicAttack = AbilityDatabase.GetInstance().GetAbilityById(weaponData.BasicAttack.Id);
        }

        public abstract object Clone();
        #endregion

        #region Getters and Setters
        public AbilityData GetPressedAbilityData(PlayerAbilityTypes _type)
        {
            AbilityData _abilityData = null;

            if (_type == PlayerAbilityTypes.BasicAttack)
            {
                _abilityData = weaponData.BasicAttack;
            }

            return _abilityData;
        }

        #endregion
    }
}



