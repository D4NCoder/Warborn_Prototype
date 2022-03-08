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
        [SerializeField] private float jumpForce = 2.0f;
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
            Controls.Player.Jump.performed += ctx => SetJump();
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

        private void SetJump()
        {
            hasPlayerJumped = true;
        }
        #endregion

        public bool TryMove(bool _isPlayerGrounded)
        {

            Vector3 _moveVector = transform.TransformDirection(new Vector3(playerMovementInput.x, 0f, playerMovementInput.y)) * movementSpeed;
            playerBody.velocity = new Vector3(_moveVector.x, playerBody.velocity.y, _moveVector.z);

            Vector2 _animatorVector = new Vector2(playerBody.velocity.normalized.z, playerBody.velocity.normalized.x);
            float _animatorSpeed = _animatorVector.magnitude;
            playerAnimator.SetFloat("forwardSpeed", _animatorSpeed);

            // Jump
            TryToJump(_isPlayerGrounded);
            return true;
        }

        private void TryToJump(bool _isPlayerGrounded)
        {
            if (shouldCheckForJump)
            {
                if (_isPlayerGrounded)
                {
                    hasPlayerJumped = false;
                    shouldCheckForJump = false;
                    playerAnimator.SetBool("hasJumped", false);
                    return;
                }
            }

            if (_isPlayerGrounded && hasPlayerJumped)
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerAnimator.SetBool("hasJumped", true);
                StartCoroutine(WaitForJump());
            }
        }

        IEnumerator WaitForJump()
        {
            yield return new WaitForSeconds(0.8f);
            shouldCheckForJump = true;
        }
    }
}
