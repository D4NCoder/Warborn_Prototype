using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Warborn.Ingame.Items.Weapons.Weapons.Core;

namespace Warborn.Ingame.Items.Weapons.Weapons.WeaponsDatabase
{
    public class WeaponDatabase : MonoBehaviour
    {
        private static WeaponDatabase Instance;
        [SerializeField] private string PathToWeaponData = "";
        public List<Weapon> Weapons;

        #region Initialization
        public void Start()
        {
            if (Instance == null) { Instance = this; }
            InitializeWeapons();
        }

        private void InitializeWeapons()
        {
            Weapons = new List<Weapon>();

            AddNewWeapon(new LightningSword(), nameof(LightningSword));
        }
        #endregion

        #region Getters
        public static WeaponDatabase GetInstance()
        {
            return Instance;
        }

        public Weapon GetWeaponById(int _id)
        {
            List<Weapon> copies = Weapons.Select(x => (Weapon)x.Clone()).ToList();

            return copies.Where(x => x.weaponData.Id == _id).FirstOrDefault();
        }
        #endregion

        private void AddNewWeapon(Weapon _weapon, string _weaponName)
        {
            _weapon.weaponData = (WeaponData)Resources.Load(PathToWeaponData + _weaponName);
            Weapons.Add(_weapon);
        }
    }
}

