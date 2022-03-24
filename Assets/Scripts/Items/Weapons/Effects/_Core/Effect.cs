using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ICloneable
{
    public EffectData effectData;

    public abstract void PerformEffect(GameObject _player);
    public abstract object Clone();
}
