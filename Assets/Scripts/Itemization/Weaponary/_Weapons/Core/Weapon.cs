using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAbilityStates
{
    BasicAttack,
    Ability1,
    Ability2,
    Ultimate
}

public abstract class Weapon : ScriptableObject
{
    [Header("Prefab")]
    public GameObject WeaponPrefab;

    [Header("Weapon Description")]
    public string Name;
    public string Description;
    public Sprite Icon;
    [Header("Abilities")]
    public Ability BasicAttack;
    public Ability Ability1;
    public Ability Ability2;
    public Ability UltimateAbility;


    public void UseAbility(PlayerAbilityStates ability)
    {
        // TODO: unsubscribe from update of the ability from the previous ability
        // TODO: subscribe to update of the ability to Update() method

        switch (ability)
        {
            case PlayerAbilityStates.BasicAttack:
                break;
            case PlayerAbilityStates.Ability1:
                break;
            case PlayerAbilityStates.Ability2:
                break;
            case PlayerAbilityStates.Ultimate:
                break;
        }
    }

    public void EquipWeapon(Transform parent)
    {
        // TODO: Instantiate it via NetworkServer 
        GameObject.Instantiate(WeaponPrefab, parent);
    }


}
