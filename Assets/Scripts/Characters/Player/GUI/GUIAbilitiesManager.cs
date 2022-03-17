using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Warborn.Characters.Player.Combat;
using Warborn.Helpers;
public class GUIAbilitiesManager : MonoBehaviour
{
    [SerializeField] PlayerAbilities abilitiesController;

    [Header("Ability1")]
    [SerializeField] Image Ability1Icon;
    [SerializeField] Image Ability1IconCooldown;

    [Header("Ability2")]
    [SerializeField] Image Ability2Icon;
    [SerializeField] Image Ability2IconCooldown;

    [Header("Ultimate Ability")]
    [SerializeField] Image UltimateAbilityIcon;
    [SerializeField] Image UltimateAbilityIconCooldown;

    [Client]
    void Update()
    {
        if (abilitiesController.GetEquipedWeapon().Ability1.CanUse)
        {
            // Is not on cooldown
            Ability1Icon.fillAmount = 1;
        }
        else
        {
            if (Ability1Icon.fillAmount == 1) { Ability1Icon.fillAmount = 0; }
            Ability1Icon.fillAmount += 1 / abilitiesController.GetEquipedWeapon().Ability1.Cooldown * Time.deltaTime;
        }



        if (Ability1Icon.sprite == abilitiesController.GetEquipedWeapon().Ability1.Icon) { return; }

        Sprite ability1Icon = abilitiesController.GetEquipedWeapon().Ability1.Icon;
        Ability1Icon.sprite = ability1Icon;
        Ability1IconCooldown.sprite = ability1Icon;

        // Sprite ability2Icon = abilitiesController.GetEquipedWeapon().Ability2.Icon;
        // Ability2Icon.sprite = ability2Icon;
        // Ability2Icon.sprite = ability2Icon;

        // Sprite ultimateAbilityIcon = abilitiesController.GetEquipedWeapon().UltimateAbility.Icon;
        // UltimateAbilityIcon.sprite = ultimateAbilityIcon;
        // UltimateAbilityIconCooldown.sprite = ultimateAbilityIcon;
    }


}
