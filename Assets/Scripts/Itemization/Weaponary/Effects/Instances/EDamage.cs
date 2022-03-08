using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warborn.Characters.Player.Statistics;

[CreateAssetMenu(fileName = "NewDamage", menuName = "Effects/DirectDamage", order = 1)]
public class EDamage : Effect
{
    public int BasicDamage;
    public override void PerformEffect(GameObject _target)
    {
        HealthController healthController;

        if ((healthController = _target.GetComponent<HealthController>()) != null)
        {
            // TODO: Get players stats and remove health based on that
            healthController.RemoveHealth(BasicDamage);
        }
    }
}
