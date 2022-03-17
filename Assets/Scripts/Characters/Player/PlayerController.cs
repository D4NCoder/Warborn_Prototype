using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Warborn.Characters.Player.Combat;
using Warborn.Characters.Player.Movement;
using Warborn.Characters.Player.Statistics;
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
        [Header("Player Stats")]
        [SerializeField] private StatsController statsController = null;
        [SerializeField] private GameObject GUI = null;

        private bool isPlayerGrounded = false;

        [Client]
        void Start()
        {
            if (!isLocalPlayer)
            {
                GUI.SetActive(false);
            }
            InitializeStats(100f, 50f, 450);
        }

        [Client]
        void FixedUpdate()
        {
            if (!isLocalPlayer) { return; }

            HandleAbilities();
            HandleMovement();
        }
        private bool HandleMovement()
        {
            return playerMover.TryMove(groundChecker.GetPlayerGrounded);
        }

        private void HandleAbilities()
        {
            playerAbilities.UpdatePlayerAbilities();
        }

        public void InitializeStats(float _movementSpeed, float _basicAttack, int _maxHealth)
        {
            statsController.MovementSpeed = _movementSpeed;
            statsController.BasicAttack = _basicAttack;
            statsController.MaxHealth = _maxHealth;
            statsController.GetHealthController().SetBaseHealth(_maxHealth);
        }

        public StatsController GetStatsController()
        {
            return statsController;
        }

    }
}
