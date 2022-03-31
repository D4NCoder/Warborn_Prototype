using UnityEngine;
using Warborn.Items.Weapons.Abilities.Core;

namespace Warborn.Items.Weapons.Weapons.Core
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/New weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject WeaponPrefab;
        [Header("Weapon Description")]
        public int Id;
        public string Name;
        public string Description;
        public Sprite Icon;
        [Header("Abilities")]
        public AbilityData BasicAttack;
        public AbilityData Ability1;
        public AbilityData Ability2;
        public AbilityData UltimateAbility;
    }
}

