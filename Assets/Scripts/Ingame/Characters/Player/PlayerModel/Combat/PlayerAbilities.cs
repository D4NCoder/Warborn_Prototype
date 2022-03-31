using Mirror;
using UnityEngine;
using System.Linq;
using Warborn.Items.Weapons.Weapons.Core;
using Warborn.Items.Weapons.Weapons.WeaponCollision;
using Warborn.Items.Weapons.Weapons.WeaponsDatabase;

namespace Warborn.Characters.Player.PlayerModel.Combat
{
    public enum PlayerAbilityTypes
    {
        BasicAttack, Ability1, Ability2, UltimateAbility
    }
    public class PlayerAbilities : NetworkBehaviour
    {
        #region Editor variables
        [Header("Equiped weapon information")]
        [SerializeField] private Weapon EquipedWeapon;
        [SerializeField] private Transform weaponPlaceholder = null;
        [SerializeField] private WeaponInstanceInfo weaponInstanceInfo = null;
        [SyncVar] public int EquipedWeaponId;
        [SyncVar] public bool hasEquipedWeapon = false;

        [Header("Abilities information")]
        [SerializeField] private PlayerAbilityTypes lastAbilityUsed;
        #endregion

        #region EquipWeapon
        public GameObject InitializeWeapon()
        {
            Weapon weapon = WeaponDatabase.GetInstance().GetWeaponById(EquipedWeaponId);
            EquipedWeapon = weapon;
            EquipedWeapon.InitializeWeapon(this.gameObject);
            return Instantiate(EquipedWeapon.weaponData.WeaponPrefab, weaponPlaceholder.position, weaponPlaceholder.rotation, weaponPlaceholder);
        }

        #region Client
        [Client]
        public void EquipWeapon(int weaponId)
        {
            CmdEquipWeaponById(weaponId);
        }

        [Client]
        public void InitWeaponIfNull()
        {
            if (hasEquipedWeapon)
            {
                if (EquipedWeapon == null)
                {
                    InitializeWeapon();
                }
            }
        }

        [TargetRpc]
        public void RpcEquipWeaponById(NetworkConnection target, int id)
        {
            InitializeWeapon();
        }
        #endregion

        #region Server
        [Command]
        public void CmdEquipWeaponById(int id)
        {
            // Go to database and search for weapon
            EquipedWeaponId = id;
            hasEquipedWeapon = true;
            GameObject _weaponPrefab;
            if ((_weaponPrefab = InitializeWeapon()) == null) { return; }

            weaponInstanceInfo = _weaponPrefab.GetComponent<WeaponInstanceInfo>();
            RpcEquipWeaponById(connectionToClient, id);
        }
        #endregion

        #endregion

        #region Perform Ability

        #region Client
        [ClientRpc]
        public void RpcPerformAbility(PlayerAbilityTypes type)
        {
            // Move this code upon the 
            EquipedWeapon.PerformAbility(type);
        }
        #endregion

        #region Server
        // Runs on Update
        [Server]
        public void UpdateWeaponsAbilities()
        {
            // Update cooldown
            if (EquipedWeapon == null) { return; }

            EquipedWeapon.CalculateAbilitiesCooldown();
        }

        [Command]
        public void CmdPerformAbility(PlayerAbilityTypes type)
        {
            lastAbilityUsed = type;
            if (EquipedWeapon.IsAbilityOnCooldown(type)) { return; }

            var data = EquipedWeapon.GetPressedAbilityData(type);
            weaponInstanceInfo.effectsToApplyToPlayer = data.EffectsToApply.Select(x => x.Id).ToList();

            RpcPerformAbility(type);
        }
        #endregion

        #endregion

        #region Event subscriptions
        [Client]
        public void OnAbilityPressed(PlayerAbilityTypes abilityType)
        {
            lastAbilityUsed = abilityType;
            CmdPerformAbility(abilityType);
        }
        #endregion
    }
}

