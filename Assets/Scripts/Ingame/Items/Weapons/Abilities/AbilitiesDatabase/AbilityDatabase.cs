using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Warborn.Items.Weapons.Abilities.Core;

namespace Warborn.Items.Weapons.Abilities.AbilitiesDatabase
{
    public class AbilityDatabase : MonoBehaviour
    {
        private static AbilityDatabase Instance;
        [SerializeField] private string PathToAbilityDatas = "";
        public List<Ability> Abilities;
        public void Start()
        {
            if (Instance == null) { Instance = this; }
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            Abilities = new List<Ability>();

            AddNewAbility(new LSBasicAttack(), nameof(LSBasicAttack));
        }

        public static AbilityDatabase GetInstance()
        {
            return Instance;
        }

        public Ability GetAbilityById(int id)
        {
            List<Ability> copies = Abilities.Select(x => (Ability)x.Clone()).ToList();

            return copies.Where(x => x.abilityData.Id == id).FirstOrDefault();
        }

        private void AddNewAbility(Ability ability, string abilityName)
        {
            ability.abilityData = (AbilityData)Resources.Load(PathToAbilityDatas + abilityName);
            ability.LoadAbilityData();
            Abilities.Add(ability);
        }
    }
}

