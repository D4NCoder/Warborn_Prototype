using TMPro;
using UnityEngine;
using Mirror;

namespace Warborn.Ingame.Map.Core.Dropper
{
    public class ItemDropperGUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshPro itemsCounterText;

        [Client]
        public void UpdateCounter(int _itemsCounter)
        {
            if (_itemsCounter == 0)
            {
                itemsCounterText.text = "No items yet. Is the Pillar activated?";
                return;
            }
            itemsCounterText.text = _itemsCounter + "x";
        }

        [ClientRpc]
        public void RpcUpdateCounter(int _itemsCounter)
        {
            UpdateCounter(_itemsCounter);
        }

    }
}

