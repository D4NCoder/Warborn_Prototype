using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/DashAndSlash", order = 1)]
public class DashAndSlash : Ability
{
    public override void PerformAbility(GameObject _localPlayer, GameObject _target)
    {
        // Set CanUse to false

        // Start counting cooldown --> Start corountine and after Cooldown secs reset CanUse to True

        // Play Animations

        // Apply Effects
    }
}
