using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Effects/New effect", order = 1)]
public class EffectData : ScriptableObject
{
    [Header("Effect Description")]
    public int Id;
    public string Name;
    public string Description;
    public Sprite Icon;
}
