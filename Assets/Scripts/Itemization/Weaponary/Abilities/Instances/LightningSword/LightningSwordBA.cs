using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/LightningSword/BasicAttack", order = 1)]
public class LightningSwordBA : Ability
{
    private float timer;
    public override void PerformAbility(GameObject _localPlayer, GameObject _target = null)
    {
        if (!canUse) { return; }
        canUse = false;
        timer = Cooldown;
        if (PlayerAnimationTriggers.Count == 1)
        {
            Debug.Log("Swing a sword");
            _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], true); // TODO: Get the name of the animation via reference in Scriptable Object
        }
        else
        {
            //CoroutineHandler.Instance.StartCoroutine(PlayQueuedAnimations(0, _localPlayer.GetComponent<Animator>()));
        }
    }

    public override void UpdateAbility(GameObject _localPlayer)
    {
        // Cooldown logic
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            canUse = true;
            timer = Cooldown;
            _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], false);
        }
    }

    //TODO: Rewrite QueuedAnimations to work

    // private IEnumerator PlayQueuedAnimations(int index, Animator animator)
    // {
    //     // Exit once all queued clips have been played
    //     if (PlayerAnimationTriggers.Count > index)
    //         yield break;

    //     string animationTrigger = PlayerAnimationTriggers[index];

    //     animator.SetBool(animationTrigger, true);
    //     animator.GetCurrentAnimatorStateInfo(0).nam

    //     // Wait till it's finished
    //     while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationTrigger))
    //     {
    //         yield return null;
    //     }

    //     CoroutineHandler.Instance.StartCoroutine(PlayQueuedAnimations(index + 1, animator));
    // }
}
