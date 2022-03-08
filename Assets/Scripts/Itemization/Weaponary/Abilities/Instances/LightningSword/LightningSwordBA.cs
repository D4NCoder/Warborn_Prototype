using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSwordBA : Ability
{
    public override void PerformAbility(GameObject _localPlayer, GameObject _target = null)
    {
        canUse = false;
        if (PlayerAnimationTriggers.Count == 1)
        {
            _localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], true); // TODO: Get the name of the animation via reference in Scriptable Object
        }
        else
        {
            CoroutineHandler.Instance.StartCoroutine(PlayQueuedAnimations(0, _localPlayer.GetComponent<Animator>()));
        }
    }

    private IEnumerator PlayQueuedAnimations(int index, Animator animator)
    {
        // Exit once all queued clips have been played
        if (PlayerAnimationTriggers.Count > index)
            yield break;

        string thisClipName = PlayerAnimationTriggers[index];

        animator.Play(thisClipName);

        // Wait till it's finished
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(thisClipName))
        {
            yield return null;
        }

        CoroutineHandler.Instance.StartCoroutine(PlayQueuedAnimations(index + 1, animator));
    }
}
