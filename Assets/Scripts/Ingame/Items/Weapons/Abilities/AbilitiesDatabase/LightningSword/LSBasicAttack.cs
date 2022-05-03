using UnityEngine;
using Warborn.Ingame.Items.Weapons.Abilities.Core;

namespace Warborn.Ingame.Items.Weapons.Abilities.AbilitiesDatabase
{
    public class LSBasicAttack : Ability
    {
        public override void PerformAbility(GameObject _localPlayer)
        {
            base.PlayAnimation("LightningSword_BA", _localPlayer);
        }

        public override object Clone()
        {
            Ability _ability = new LSBasicAttack();
            _ability.AbilityData = base.AbilityData;
            _ability.LoadAbilityData();

            return _ability;
        }
    }
}

