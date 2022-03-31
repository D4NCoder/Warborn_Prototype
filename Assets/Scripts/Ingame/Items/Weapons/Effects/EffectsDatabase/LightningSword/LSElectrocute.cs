using UnityEngine;
using Warborn.Items.Weapons.Effects.Core;

namespace Warborn.Items.Weapons.Effects.EffectsDatabase
{
    public class LSElectrocute : Effect
    {
        public override void PerformEffect(GameObject _Player)
        {
            Debug.Log("LSElectrocute effect performed.");
        }

        public override object Clone()
        {
            Effect effect = new LSElectrocute();
            effect.effectData = base.effectData;

            return effect;
        }
    }
}

