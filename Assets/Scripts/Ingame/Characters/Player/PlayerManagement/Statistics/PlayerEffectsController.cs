using UnityEngine;
using Mirror;
using System.Collections.Generic;
using Warborn.Ingame.Items.Weapons.Effects.Core;

namespace Warborn.Ingame.Characters.Player.PlayerManagement.Statistics
{
    public class PlayerEffectsController : NetworkBehaviour
    {
        public List<Effect> effects = new List<Effect>();

        [Server]
        public void OnEffectsApply(GameObject _playerToApply)
        {
            if (effects.Count <= 0) { return; }

            foreach (Effect _effect in effects)
            {
                _effect.PerformEffect(_playerToApply);
            }
            effects = new List<Effect>();
        }
    }

}
