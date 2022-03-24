using UnityEngine;
using Mirror;


namespace Warborn.Characters.Player.Combat
{
    public enum PlayerAbilityTypes
    {
        BasicAttack, Ability1, Ability2, UltimateAbility
    }
    public class PlayerAbilities : NetworkBehaviour
    {
        #region Editor variables
        [Header("References")]
        [SerializeField] private Transform weaponPlaceholder = null;

        [Header("EquipedWeapon")]
        [SerializeField] private Weapon EquipedWeapon;

        [SyncVar]
        public int EquipedWeaponId;
        [SyncVar]
        public bool hasEquipedWeapon = false;

        [SerializeField] private PlayerAbilityTypes lastAbilityUsed;
        #endregion

        #region Event subscriptions
        [Client]
        public void OnAbilityPressed(PlayerAbilityTypes abilityType)
        {
            lastAbilityUsed = abilityType;
            CmdPerformAbility(abilityType);
        }
        #endregion

        #region Client

        #region Equip Weapon
        [Client]
        public void EquipWeapon()
        {
            CmdEquipWeaponById(WeaponInstances.LIGHTNING_SWORD);
        }

        public Weapon GetEquipedWeapon()
        {
            return EquipedWeapon;
        }

        [Client]
        public void ReequipWeaponOnClientFromServer()
        {
            Weapon weapon = WeaponDatabase.GetInstance().GetWeaponById(EquipedWeaponId);
            EquipedWeapon = weapon;
            EquipedWeapon.InitializeWeapon(this.gameObject);
            GameObject _weaponPrefab = Instantiate(EquipedWeapon.weaponData.WeaponPrefab, weaponPlaceholder.position, weaponPlaceholder.rotation, weaponPlaceholder);
        }

        [TargetRpc]
        public void RpcEquipWeaponById(NetworkConnection target, int id)
        {
            Weapon weapon = WeaponDatabase.GetInstance().GetWeaponById(id);
            EquipedWeapon = weapon;
            EquipedWeapon.InitializeWeapon(this.gameObject);
            GameObject _weaponPrefab = Instantiate(EquipedWeapon.weaponData.WeaponPrefab, weaponPlaceholder.position, weaponPlaceholder.rotation, weaponPlaceholder);
        }
        #endregion

        #region Perform Ability
        [ClientRpc]
        public void RpcPerformAbility(PlayerAbilityTypes type)
        {
            // Move this code upon the 

            EquipedWeapon.PerformAbility(type);
        }
        #endregion

        #endregion

        #region Server

        #region Perform Ability
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

            RpcPerformAbility(type);
        }
        #endregion

        #region Equip Weapon
        [Command]
        public void CmdEquipWeaponById(int id)
        {
            // Go to database and search for weapon
            Weapon weapon = WeaponDatabase.GetInstance().GetWeaponById(id);
            EquipedWeaponId = id;
            hasEquipedWeapon = true;
            if (weapon == null) { return; }
            EquipedWeapon = weapon;
            EquipedWeapon.InitializeWeapon(this.gameObject);
            GameObject _weaponPrefab = Instantiate(EquipedWeapon.weaponData.WeaponPrefab, weaponPlaceholder.position, weaponPlaceholder.rotation, weaponPlaceholder);
            RpcEquipWeaponById(connectionToClient, id);
        }
        #endregion

        #endregion
    }
}

