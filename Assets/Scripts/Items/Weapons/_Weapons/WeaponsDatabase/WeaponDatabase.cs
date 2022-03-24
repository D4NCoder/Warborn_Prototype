using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponDatabase : MonoBehaviour
{
    private static WeaponDatabase Instance;
    [SerializeField] private string PathToWeaponDatas = "";
    public List<Weapon> Weapons;
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

    public static WeaponDatabase GetInstance()
    {
        return Instance;
    }

    public Weapon GetWeaponById(int id)
    {
        List<Weapon> copies = Weapons.Select(x => (Weapon)x.Clone()).ToList();

        return copies.Where(x => x.weaponData.Id == id).FirstOrDefault();
    }

    private void AddNewWeapon(Weapon weapon, string weaponName)
    {
        weapon.weaponData = (WeaponData)Resources.Load(PathToWeaponDatas + weaponName);
        Weapons.Add(weapon);
    }
}
