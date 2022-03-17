using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : ScriptableObject
{
    [Header("Ability Description")]
    public string Name;
    public string Description;
    public Sprite Icon;

    [Header("Collections")]
    public List<string> PlayerAnimationTriggers;
    public List<Effect> EffectsToApply;

    [Header("Cooldown Handling")]
    public float Timer;
    public float Cooldown;

    [Header("Specifics")]
    public bool CanUse;
    public bool EffectsApplied;

    protected GameObject localPlayer;

    public abstract void PerformAbility(GameObject _localPlayer, GameObject _target);

    public abstract void UpdateAbility();

    public virtual void StartCooldown(Weapon weapon)
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            OnAbilityEnd();
            CanUse = true;
            Timer = Cooldown;
            EffectsApplied = false;
            weapon.onCooldown -= StartCooldown;
        }
    }
    public abstract void OnAbilityEnd();

    protected void SetLocalPlayer(GameObject _localPlayer)
    {
        localPlayer = _localPlayer;
    }
}
