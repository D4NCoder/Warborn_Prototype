using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Warborn.Ingame.Items.Weapons.Abilities.Core;

namespace Warborn.Ingame.Items.Weapons.Abilities.AbilitiesDatabase
{
    public class AbilityDatabase : MonoBehaviour
    {
        private static AbilityDatabase Instance;
        [SerializeField] private string PathToAbilityDatas = "";
        public List<Ability> Abilities;

        #region Initialization
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
        #endregion

        #region Getters
        public static AbilityDatabase GetInstance()
        {
            return Instance;
        }
        public Ability GetAbilityById(int _id)
        {
            List<Ability> _copies = Abilities.Select(x => (Ability)x.Clone()).ToList();

            return _copies.Where(x => x.AbilityData.Id == _id).FirstOrDefault();
        }
        #endregion

        private void AddNewAbility(Ability _ability, string _abilityName)
        {
            _ability.AbilityData = (AbilityData)Resources.Load(PathToAbilityDatas + _abilityName);
            _ability.LoadAbilityData();
            Abilities.Add(_ability);
        }
    }
}

