using Warborn.Items.Weapons.Weapons.Core;

namespace Warborn.Items.Weapons.Weapons.WeaponsDatabase
{
    public class LightningSword : Weapon
    {
        public override object Clone()
        {
            Weapon weapon = new LightningSword();
            weapon.weaponData = base.weaponData;

            return weapon;
        }
    }
}

