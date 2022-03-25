using UnityEngine;
using Warborn.Items.Weapons.Abilities.Core;

namespace Warborn.Items.Weapons.Abilities.AbilitiesDatabase
{
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
}

