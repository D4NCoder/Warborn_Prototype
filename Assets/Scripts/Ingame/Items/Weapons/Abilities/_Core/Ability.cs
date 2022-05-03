using UnityEngine;
using System;

namespace Warborn.Ingame.Items.Weapons.Abilities.Core
{
    public abstract class Ability : ICloneable
    {
        #region Class members
        public AbilityData AbilityData;
        private bool isOnCooldown = false;
        private float timer;

        public event Action<Ability> onCooldownDone;
        #endregion

        #region Initialization
        public virtual void LoadAbilityData()
        {
            timer = AbilityData.BasicCooldown;
        }

        public abstract object Clone();
        #endregion

        #region Perfrom ability with animations
        public abstract void PerformAbility(GameObject _localPlayer);

        protected void PlayAnimation(string _animationTrigger, GameObject _player)
        {
            foreach (string _animation in AbilityData.PlayerAnimationTriggers)
            {
                if (_animation == _animationTrigger)
                {
                    _player.GetComponent<Animator>().SetTrigger(_animation);
                }
            }
        }
        #endregion

        #region Cooldown
        public bool IsOnCooldown()
        {
            return isOnCooldown;
        }

        public void SetCooldown(bool _value)
        {
            isOnCooldown = _value;
        }
        public void CalculateCooldown()
        {
            if (!isOnCooldown) { return; }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isOnCooldown = false;
                timer = AbilityData.BasicCooldown;
                onCooldownDone?.Invoke(this);
            }
        }
        #endregion


    }
}

