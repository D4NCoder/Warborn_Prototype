using UnityEngine;
using Mirror;
using System.Collections.Generic;
using Warborn.Items.Weapons.Effects.Core;

namespace Warborn.Characters.Player.PlayerManagement.Statistics
{
    public class PlayerEffectsController : NetworkBehaviour
    {
        public List<Effect> effects = new List<Effect>();
        public string ahoj;

        [Server]
        public void OnEffectsApply(GameObject player1)
        {
            if (effects.Count <= 0) { return; }

            foreach (Effect effect in effects)
            {
                effect.PerformEffect(player1);
            }
        }
    }

}
