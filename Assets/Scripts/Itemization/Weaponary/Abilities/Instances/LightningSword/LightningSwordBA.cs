using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/LightningSword/BasicAttack", order = 1)]
public class LightningSwordBA : Ability
{
    public override void PerformAbility(GameObject _localPlayer, GameObject _target = null)
    {
        if (!CanUse) { return; }
        CanUse = false;
        SetLocalPlayer(_localPlayer);

        if (PlayerAnimationTriggers.Count == 1)
        {
            Debug.Log("Swing a sword");
            localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], true); // TODO: Get the name of the animation via reference in Scriptable Object
        }
        else
        {
            //CoroutineHandler.Instance.StartCoroutine(PlayQueuedAnimations(0, _localPlayer.GetComponent<Animator>()));
        }
    }

    public override void UpdateAbility()
    {

    }

    public override void OnAbilityEnd()
    {
        localPlayer.GetComponent<Animator>().SetBool(PlayerAnimationTriggers[0], false);
    }
}
