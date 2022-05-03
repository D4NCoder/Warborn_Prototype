using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerManagement.Statistics;
using Warborn.Ingame.Items.Weapons.Effects.Core;

namespace Warborn.Ingame.Items.Weapons.Effects.EffectsDatabase
{
    public class LSElectrocute : Effect
    {
        public override void PerformEffect(GameObject _Player)
        {
            PlayerStatsController _statsController = _Player.GetComponent<PlayerStatsController>();
            Debug.Log(_Player.gameObject.name + " has been hit");
            _statsController.ChangeHealth(_statsController.CurrentHealth - 80);
        }

        public override object Clone()
        {
            Effect _effect = new LSElectrocute();
            _effect.effectData = base.effectData;

            return _effect;
        }
    }
}

