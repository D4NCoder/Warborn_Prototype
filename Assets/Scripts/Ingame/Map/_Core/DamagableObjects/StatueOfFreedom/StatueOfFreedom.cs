using UnityEngine;
using Mirror;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;
using Warborn.Ingame.Items.Weapons.Weapons.WeaponCollision;
using Warborn.Ingame.Characters.Player.PlayerModel.Core;
using Warborn.Ingame.Networking;

namespace Warborn.Ingame.Map.Core.DamagableObjects.StatueOfFreedom
{
    public class StatueOfFreedom : DamagableObject
    {
        public override void DoDamage()
        {
            throw new System.NotImplementedException();
        }

        [Server]
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.layer != CollisionType.WEAPON) { return; }

            WeaponInstanceInfo _instanceInfo = _other.gameObject.GetComponent<WeaponInstanceInfo>();
            if (_instanceInfo.BelongingTeam == BelongingTeam) { return; }

            int _damage = _other.GetComponent<WeaponInstanceInfo>().BasicDamage;

            RecieveDamage(_damage);
        }
    }
}

