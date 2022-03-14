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

    [Header("Specifics")]
    public bool canUse;
    public float Cooldown;

    public abstract void PerformAbility(GameObject _localPlayer, GameObject _target);

    public abstract void UpdateAbility(GameObject _localPlayer);
}
