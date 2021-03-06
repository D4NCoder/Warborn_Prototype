using Mirror;
using UnityEngine;
using System.Linq;
using Warborn.Ingame.Items.Weapons.Weapons.Core;
using Warborn.Ingame.Items.Weapons.Weapons.WeaponCollision;
using Warborn.Ingame.Items.Weapons.Weapons.WeaponsDatabase;
using System;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;

namespace Warborn.Ingame.Characters.Player.PlayerModel.Combat
{
    public enum PlayerAbilityTypes
    {
        BasicAttack, Ability1, Ability2, UltimateAbility
    }
    public class PlayerAbilities : NetworkBehaviour
    {
        #region Variables and Properties

        #region References
        [Header("Equiped weapon information")]
        [SerializeField] private Weapon EquipedWeapon;
        [SerializeField] private Transform weaponPlaceholder = null;

        [SerializeField] private WeaponInstanceInfo weaponInstanceInfo = null;
        public WeaponInstanceInfo WeaponInstanceInfo => weaponInstanceInfo;

        #endregion

        #region Weapon information
        [SyncVar] public int EquipedWeaponId;
        [SyncVar] public bool hasEquipedWeapon = false;

        [Header("Abilities information")]
        [SerializeField] private PlayerAbilityTypes lastAbilityUsed;
        #endregion

        #endregion

        #region Server Events

        public event Action<WeaponInstanceInfo> onWeaponEquiped;

        #endregion

        #region EquipWeapon
        public GameObject InitializeWeapon()
        {
            if (EquipedWeapon != null) { return null; }
            Weapon _weapon = WeaponDatabase.GetInstance().GetWeaponById(EquipedWeaponId);
            EquipedWeapon = _weapon;
            EquipedWeapon.InitializeWeapon(this.gameObject);
            return Instantiate(EquipedWeapon.weaponData.WeaponPrefab, weaponPlaceholder.position, weaponPlaceholder.rotation, weaponPlaceholder);
        }

        #region Client
        [Client]
        public void EquipWeapon(int _weaponId)
        {
            CmdEquipWeaponById(_weaponId);
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
        public void RpcEquipWeaponById(NetworkConnection _connection)
        {
            InitializeWeapon();
        }
        #endregion

        #region Server
        [Command]
        public void CmdEquipWeaponById(int _weaponId)
        {
            GameObject _weaponPrefab;
            EquipedWeaponId = _weaponId;
            hasEquipedWeapon = true;

            if ((_weaponPrefab = InitializeWeapon()) == null) { return; }
            weaponInstanceInfo = _weaponPrefab.GetComponent<WeaponInstanceInfo>();
            onWeaponEquiped?.Invoke(weaponInstanceInfo);

            RpcEquipWeaponById(connectionToClient);
        }
        #endregion

        #endregion

        #region Perform Ability

        #region Client
        [ClientRpc]
        public void RpcPerformAbility(PlayerAbilityTypes _type)
        {
            // Move this code upon the 
            EquipedWeapon.PerformAbility(_type);
            CmdSetWaitingTimeForWeaponTrigger();
        }
        #endregion

        #region Server
        // Runs on Update
        [Server]
        public void UpdateWeaponsAbilities()
        {
            if (EquipedWeapon == null) { return; }

            // Update cooldown
            EquipedWeapon.CalculateAbilitiesCooldown();
        }

        [Command]
        public void CmdPerformAbility(PlayerAbilityTypes _type)
        {
            lastAbilityUsed = _type;
            if (EquipedWeapon.IsAbilityOnCooldown(_type)) { return; }

            var _data = EquipedWeapon.GetPressedAbilityData(_type);
            weaponInstanceInfo.EffectsToApplyToPlayer = _data.EffectsToApply.Select(x => x.Id).ToList();

            weaponInstanceInfo.HasPlayerAttacked = true;
            weaponInstanceInfo.ChangeWeaponToTriggerable();

            RpcPerformAbility(_type);
        }

        [Command]
        public void CmdSetWaitingTimeForWeaponTrigger()
        {
            if (!hasAuthority) { return; }
            weaponInstanceInfo.WaitTimeForResetCounter = this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        }
        #endregion
        #endregion

        #region Event handlers
        [Client]
        public void OnAbilityPressed(PlayerAbilityTypes _abilityType)
        {
            lastAbilityUsed = _abilityType;
            CmdPerformAbility(_abilityType);
        }

        [Server]
        public void OnAttackDamageChange(int _attackDamage)
        {
            weaponInstanceInfo.BasicDamage = _attackDamage;
        }

        #endregion
    }
}

