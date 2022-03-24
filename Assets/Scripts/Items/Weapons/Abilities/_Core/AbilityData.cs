using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Abilities/New ability", order = 1)]
public class AbilityData : ScriptableObject
{
    [Header("Ability Description")]
    public int Id;
    public string Name;
    public string Description;
    public Sprite Icon;

    [Header("Collections")]
    public List<string> PlayerAnimationTriggers;
    public List<EffectData> EffectsToApply;

    [Header("Cooldown")]
    public float BasicCooldown;
}
