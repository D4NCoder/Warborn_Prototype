using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Warborn_Prototype.Inputs;

namespace Warborn.Characters.Player.Movement
{
    public class PlayerMover : MonoBehaviour
    {
        #region  Variables
        [Header("References")]
        [SerializeField] private Rigidbody playerBody = null;
        [SerializeField] private Animator playerAnimator = null;
        [Header("Initial variables")]
        [SerializeField] private float movementSpeed = 7f;
        [SerializeField] private bool isPlayerMooving = false;
        [SerializeField] private bool hasPlayerJumped = false;

        private bool shouldCheckForJump = false;

        private Vector2 playerMovementInput;
        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }
        #endregion

        #region Initializing Controls
        private void OnEnable() => Controls.Enable();
        private void OnDisable() => Controls.Disable();
        private void Start()
        {
            InitControlsCallbacks();
        }
        private void InitControlsCallbacks()
        {
            Controls.Player.Move.performed += ctx => StartMoving(ctx.ReadValue<Vector2>());
            Controls.Player.Move.canceled += ctx => CancelMovement();
        }
        #endregion

        #region Controls callbacks
        private void StartMoving(Vector2 _movementVector)
        {
            playerMovementInput = _movementVector;
            isPlayerMooving = true;
        }
        private void CancelMovement()
        {
            playerMovementInput = Vector2.zero;
            isPlayerMooving = false;
        }

        #endregion

        public bool TryMove(bool _isPlayerGrounded)
        {

            Vector3 _moveVector = transform.TransformDirection(new Vector3(playerMovementInput.x, 0f, playerMovementInput.y)) * movementSpeed;
            playerBody.velocity = new Vector3(_moveVector.x, playerBody.velocity.y, _moveVector.z);

            Vector2 _animatorVector = new Vector2(playerBody.velocity.normalized.z, playerBody.velocity.normalized.x);
            float _animatorSpeed = _animatorVector.magnitude;
            playerAnimator.SetFloat("forwardSpeed", _animatorSpeed);

            return true;
        }
    }
}
