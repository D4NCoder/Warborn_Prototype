using System;
using UnityEngine;

namespace Warborn.Ingame.Items.Weapons.Effects.Core
{
    public abstract class Effect : ICloneable
    {
        public EffectData effectData;

        public abstract void PerformEffect(GameObject _player);
        public abstract object Clone();
    }
}

