using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warborn.Characters.Player.Statistics
{
    public class EffectsController : MonoBehaviour
    {
        public List<Effect> CurrentEffects = new List<Effect>();
        private void Update()
        {
            if (CurrentEffects.Count < 1) { return; }

            Effect[] effects = CurrentEffects.ToArray();

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].DurationTime == 0)
                {
                    // Instant response and deletion of the effect
                    effects[i].PerformEffect(this.gameObject);
                    CurrentEffects.Remove(effects[i]);
                }
                else
                {
                    // Apply effect for X times for Y seconds
                    // TODO: Make it server Authoritative
                    StartCoroutine(PerformEffectMultipleTimes(effects[i], effects[i].TimesToPerform, (float)(effects[i].DurationTime / effects[i].TimesToPerform)));
                }
            }
        }

        private IEnumerator PerformEffectMultipleTimes(Effect effect, int times, float time)
        {
            if (times <= 0) { yield break; } // I dont know what that is going to couse
            effect.PerformEffect(this.gameObject);

            yield return new WaitForSeconds(time);
            StartCoroutine(PerformEffectMultipleTimes(effect, times - 1, time));
        }
    }
}

