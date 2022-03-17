using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/LightningSword/DashAndSlash", order = 1)]
public class LightningSwordDashAndSlash : Ability
{
    [Header("Dash information")]
    public float DashSpeed;
    private bool dashed = false;

    public override void PerformAbility(GameObject _localPlayer, GameObject _target)
    {
        // Set CanUse to false
        CanUse = false;
        Timer = Cooldown;
        SetLocalPlayer(_localPlayer);
        _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[1], false);
        _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], true);

    }
    public override void UpdateAbility()
    {
        if (CanUse) { return; }
        if (Cooldown - Timer <= 0.2)
        {
            Debug.Log("Dashing");
            Rigidbody localPlayerRb = localPlayer.GetComponent<Rigidbody>();
            localPlayerRb.AddForce(localPlayer.transform.forward * DashSpeed, ForceMode.Impulse);
        }
        else
        {
            localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], false);
            localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[1], true);
        }
    }
    public override void OnAbilityEnd()
    {
        localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[1], false);
    }

}
