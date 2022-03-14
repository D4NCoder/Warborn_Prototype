using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/LightningSword/DashAndSlash", order = 1)]
public class LightningSwordDashAndSlash : Ability
{
    [Header("Dash information")]
    public float DashSpeed;
    private bool dashed = false;

    private float timer;
    public override void PerformAbility(GameObject _localPlayer, GameObject _target)
    {
        // Set CanUse to false
        canUse = false;
        timer = Cooldown;
        _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[1], false);
        _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], true);

        // Dash animation
        // Start the dash animation
        // move player to certain destination
        // Slash animation
        // Deal damage

    }

    public override void UpdateAbility(GameObject _localPlayer)
    {
        timer -= Time.deltaTime;
        if (Cooldown - timer <= 0.2)
        {
            Rigidbody localPlayerRb = _localPlayer.GetComponent<Rigidbody>();
            localPlayerRb.AddForce(_localPlayer.transform.forward * DashSpeed, ForceMode.Impulse);

        }
        else
        {
            _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], false);
            _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[1], true);
        }

        if (timer <= 0)
        {
            canUse = true;
            timer = Cooldown;
        }
    }
}
