using System;
using UnityEngine;
using Warborn.Characters.Player.Combat;

public abstract class Weapon : ICloneable
{
    public WeaponData weaponData;

    public Ability BasicAttack;

    public GameObject LocalPlayer;

    private event Action onCooldownAbilities;

    #region Performing the ability of a weapon
    public void PerformAbility(PlayerAbilityTypes ability)
    {
        if (ability == PlayerAbilityTypes.BasicAttack)
        {
            BasicAttack.PerformAbility(LocalPlayer);
            onCooldownAbilities += BasicAttack.CalculateCooldown;
            BasicAttack.onCooldownDone += OnAbilityCooldownEnded;
            BasicAttack.SetCooldown(true);
        }
    }
    public bool IsAbilityOnCooldown(PlayerAbilityTypes type)
    {
        bool valueToReturn = false;
        if (type == PlayerAbilityTypes.BasicAttack)
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

    private void OnAbilityCooldownEnded(Ability ability)
    {
        onCooldownAbilities -= ability.CalculateCooldown;
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
        // Go to database of Abilities and assign them
        BasicAttack = AbilityDatabase.GetInstance().GetAbilityById(weaponData.BasicAttack.Id);
    }

    public abstract object Clone();
    #endregion
}

