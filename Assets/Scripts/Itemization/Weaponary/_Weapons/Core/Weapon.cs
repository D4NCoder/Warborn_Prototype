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
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/New Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    #region Weapon information
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

    public bool AnAbilityUsed = false;
    #endregion

    private delegate void UpdateAbility(GameObject _localPlayer);
    private UpdateAbility UpdateChosenAbility = null;
    private GameObject localPlayer = null;

    public void UseAbility(PlayerAbilityStates _ability, GameObject _localPlayer)
    {
        if (localPlayer == null) { localPlayer = _localPlayer; }
        switch (_ability)
        {
            case PlayerAbilityStates.BasicAttack:
                AnAbilityUsed = true;
                BasicAttack.PerformAbility(localPlayer, localPlayer);
                UpdateChosenAbility = BasicAttack.UpdateAbility;
                break;
            case PlayerAbilityStates.Ability1:
                Ability1.PerformAbility(localPlayer, localPlayer);
                UpdateChosenAbility = Ability1.UpdateAbility;
                break;
            case PlayerAbilityStates.Ability2:
                Debug.Log("Pressed E");
                break;
            case PlayerAbilityStates.Ultimate:
                Debug.Log("Pressed R");
                break;
        }
    }

    public void EquipWeapon(Transform parent)
    {
        // TODO: Instantiate it via NetworkServer
        GameObject.Instantiate(WeaponPrefab, parent);
    }

    public void UpdateWeapon()
    {
        if (AnAbilityUsed)
        {
            if (BasicAttack.canUse && Ability1.canUse)
            {
                AnAbilityUsed = false;
            }
        }

        // any logic that needs to be updated per frame
        if (UpdateChosenAbility != null && AnAbilityUsed)
        {
            UpdateChosenAbility(localPlayer);
        }

    }


}
