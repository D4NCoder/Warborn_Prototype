using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warborn.Characters.Player;
using Warborn.Characters.Player.Statistics;

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
    #endregion


    private GameObject localPlayer = null;
    private GameObject WeaponInstance = null;
    private Ability lastUsedAbility;

    /*--------Events and Delegates--------*/
    private delegate void UpdateAbility();
    private UpdateAbility UpdateChosenAbility = null;
    public event Action<Weapon> onCooldown;

    public void UseAbility(PlayerAbilityStates _ability, GameObject _localPlayer)
    {
        if (localPlayer == null) { localPlayer = _localPlayer; }
        switch (_ability)
        {
            case PlayerAbilityStates.BasicAttack:
                if (BasicAttack.CanUse)
                {
                    BasicAttack.PerformAbility(localPlayer, localPlayer);
                    UpdateChosenAbility = BasicAttack.UpdateAbility;
                    onCooldown += BasicAttack.StartCooldown;
                    lastUsedAbility = BasicAttack;
                }
                break;
            case PlayerAbilityStates.Ability1:
                if (Ability1.CanUse)
                {
                    Ability1.PerformAbility(localPlayer, localPlayer);
                    UpdateChosenAbility = Ability1.UpdateAbility;
                    onCooldown += Ability1.StartCooldown;
                    lastUsedAbility = Ability1;
                }
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
        WeaponInstance = GameObject.Instantiate(WeaponPrefab, parent);
        WeaponInstance.GetComponent<WeaponCollisionController>().onHit += OnHitWeapon;
    }

    public void UpdateWeapon()
    {
        onCooldown?.Invoke(this);
        // any logic that needs to be updated per frame
        if (UpdateChosenAbility != null)
        {
            UpdateChosenAbility();
        }
    }

    private void OnHitWeapon(GameObject enemy)
    {
        if (lastUsedAbility == null) { return; }
        if (lastUsedAbility.EffectsApplied) { return; }
        if (lastUsedAbility.CanUse) { return; }

        Debug.Log("Trying to hit player: " + enemy.name);

        // EffectsController effectsController = enemy.GetComponent<PlayerController>().GetStatsController().GetEffectsController();
        // effectsController.AddEffects(lastUsedAbility.EffectsToApply);
        // lastUsedAbility.EffectsApplied = true;
    }

}
