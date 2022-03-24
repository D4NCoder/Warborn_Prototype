using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSElectrocute : Effect
{
    public override void PerformEffect(GameObject _Player)
    {
        Debug.Log("LSElectrocute effect performed.");
    }

    public override object Clone()
    {
        Effect effect = new LSElectrocute();
        effect.effectData = base.effectData;

        return effect;
    }
}
