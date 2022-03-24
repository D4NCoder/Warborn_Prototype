using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSword : Weapon
{
    public override object Clone()
    {
        Weapon weapon = new LightningSword();
        weapon.weaponData = base.weaponData;

        return weapon;
    }
}
