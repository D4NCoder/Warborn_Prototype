using UnityEngine;

namespace Warborn.Ingame.Items.CraftingItems
{
    [CreateAssetMenu(fileName = "CraftingItemSO", menuName = "Items/New item", order = 1)]
    public class CraftingItemSO : ScriptableObject
    {
        public string Name;
        public string Description;
        public GameObject Prefab;
    }

}
