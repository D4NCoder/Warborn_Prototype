using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Warborn.Characters.Player.Combat;
using Warborn.Characters.Player.Movement;
using Warborn_Prototype.Inputs;

namespace Warborn.Characters.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("Player Movement")]
        [SerializeField] private PlayerMover playerMover = null;
        [SerializeField] private GroundChecker groundChecker = null;
        [Header("Player Combat")]
        [SerializeField] private PlayerAbilities playerAbilities = null;
        [Header("Player Health")]
        [SerializeField] private Slider slider = null;
        private bool isPlayerGrounded = false;



        [Client]
        void FixedUpdate()
        {
            if (!isLocalPlayer) { return; }

            if (HandleAbilities()) { return; }
            HandleMovement();

        }


        private bool HandleMovement()
        {
            return playerMover.TryMove(groundChecker.GetPlayerGrounded);
        }

        private bool HandleAbilities()
        {
            return false;
            //return playerAbilities.TryUseAbility();
        }

    }
}
