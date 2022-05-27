using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerManagement.Statistics;
using Warborn.Ingame.Items.Weapons.Effects.Core;

namespace Warborn.Ingame.Items.Weapons.Effects.EffectsDatabase
{
    public class SOFBasicAttack : Effect
    {
        public override void PerformEffect(GameObject _Player)
        {
            PlayerStatsController _statsController = _Player.GetComponent<PlayerStatsController>();
            Debug.Log(_Player.gameObject.name + " has been hit by a Statue of Freedom");
            _statsController.ChangeHealth(_statsController.CurrentHealth - 50);
        }

        public override object Clone()
        {
            Effect _effect = new SOFBasicAttack();
            _effect.effectData = base.effectData;

            return _effect;
        }
    }
}

