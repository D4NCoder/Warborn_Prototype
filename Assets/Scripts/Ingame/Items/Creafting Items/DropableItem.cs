using UnityEngine;
using Mirror;
using System;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;

namespace Warborn.Ingame.Items.CraftingItems
{
    [RequireComponent(typeof(BoxCollider))]
    public class DropableItem : NetworkBehaviour
    {
        public event Action onPickupItem;

        [Server]
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.layer != CollisionType.PLAYER) { return; }

            onPickupItem?.Invoke();
        }

    }
}


