using UnityEngine;
using UnityEngine.UI;

public abstract class Effect : ScriptableObject
{
    [Header("Effect Description")]
    public string Name;
    public string Description;
    public Sprite Icon;
    [Header("Specifics")]
    public float DurationTime;
    public int TimesToPerform;

    public abstract void PerformEffect(GameObject _target);
}
