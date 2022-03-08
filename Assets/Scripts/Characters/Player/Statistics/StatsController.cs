using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Warborn.Characters.Player.Statistics
{
    public class StatsController : NetworkBehaviour
    {
        [SerializeField] private HealthController healthController;

        public HealthController GetHealthController()
        {
            return healthController;
        }
    }
}


