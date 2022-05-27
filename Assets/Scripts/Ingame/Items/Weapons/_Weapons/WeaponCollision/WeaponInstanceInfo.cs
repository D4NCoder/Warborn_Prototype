using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;
using Warborn.Ingame.Map.Core;

namespace Warborn.Ingame.Items.Weapons.Weapons.WeaponCollision
{
    public class WeaponInstanceInfo : MonoBehaviour
    {
        [SerializeField] private List<int> effectsToApplyToPlayer;
        public List<int> EffectsToApplyToPlayer { get { return effectsToApplyToPlayer; } set { effectsToApplyToPlayer = value; } }

        [SerializeField] private int basicDamage;
        public int BasicDamage { get { return basicDamage; } set { basicDamage = value; } }

        [SerializeField] private EntityTeamType belongingTeam;
        public EntityTeamType BelongingTeam { get { return belongingTeam; } set { belongingTeam = value; } }

        [SerializeField] private bool hasPlayerAttack = false;
        public bool HasPlayerAttacked { get { return hasPlayerAttack; } set { hasPlayerAttack = value; } }

        private int triggerCounter = 0;

        [SerializeField] public float WaitTimeForResetCounter { get; set; } = 0f;

        public void ResetCounter()
        {
            triggerCounter = 0;
            hasPlayerAttack = false;
        }


        [Server]
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.layer == CollisionType.PLAYER && this.transform.IsChildOf(_other.gameObject.transform)) { return; }
            if (!hasPlayerAttack) { return; }

            if (_other.gameObject.layer == CollisionType.PLAYER || _other.gameObject.layer == CollisionType.TARGETABLE)
            {
                if (triggerCounter < 1)
                {
                    Invoke(nameof(ResetCounter), WaitTimeForResetCounter);
                }
                else
                {
                    this.gameObject.layer = CollisionType.NONTRIGGERABLE;
                }

                triggerCounter++;

            }
        }

        public void ChangeWeaponToTriggerable()
        {
            this.gameObject.layer = CollisionType.WEAPON;
            Invoke(nameof(CheckForTrigger), WaitTimeForResetCounter);
        }

        private void CheckForTrigger()
        {
            if (triggerCounter == 0)
            {
                hasPlayerAttack = false;
                this.gameObject.layer = CollisionType.NONTRIGGERABLE;
            }
        }



    }
}
