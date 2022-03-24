using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Ability : ICloneable
{
    public AbilityData abilityData;
    private bool isOnCooldown = false;
    private float timer;

    public event Action<Ability> onCooldownDone;

    public abstract void PerformAbility(GameObject localPlayer);

    protected void PlayAnimation(string animationTrigger, GameObject player)
    {
        foreach (string animation in abilityData.PlayerAnimationTriggers)
        {
            if (animation == animationTrigger)
            {
                player.GetComponent<Animator>().SetTrigger(animation);
            }
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public void SetCooldown(bool value)
    {
        isOnCooldown = value;
    }
    public void CalculateCooldown()
    {
        if (!isOnCooldown) { return; }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isOnCooldown = false;
            timer = abilityData.BasicCooldown;
            onCooldownDone?.Invoke(this);
        }
    }

    public virtual void LoadAbilityData()
    {
        timer = abilityData.BasicCooldown;
    }

    public abstract object Clone();

}
