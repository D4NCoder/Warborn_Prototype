using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSBasicAttack : Ability
{
    public override void PerformAbility(GameObject localPlayer)
    {
        base.PlayAnimation("LightningSword_BA", localPlayer);
    }

    public override object Clone()
    {
        Ability ability = new LSBasicAttack();
        ability.abilityData = base.abilityData;
        ability.LoadAbilityData();

        return ability;
    }
}
