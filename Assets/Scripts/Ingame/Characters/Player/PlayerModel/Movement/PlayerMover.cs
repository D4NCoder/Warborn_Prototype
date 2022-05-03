using Mirror;
using UnityEngine;

namespace Warborn.Ingame.Characters.Player.PlayerModel.Movement
{
    public class PlayerMover : NetworkBehaviour
    {
        #region  Variables and Properties

        #region References
        [Header("References")]
        [SerializeField] private Rigidbody playerBody = null;
        [SerializeField] private Animator playerAnimator = null;
        #endregion

        #region Initial variables
        [Header("Initial variables")]
        [SerializeField] private float movementSpeed = 7f;

        [SyncVar]
        [SerializeField] private bool isPlayerGrounded = false;
        #endregion

        private Vector2 playerMovementInput;
        #endregion
        #region Events handlers
        public void StartMoving(Vector2 _movementVector)
        {
            playerMovementInput = _movementVector;
        }

        public void CancelMovement()
        {
            playerMovementInput = Vector2.zero;
        }
        [Command]
        public void CmdOnHitGround(bool _isPlayerGrounded)
        {
            isPlayerGrounded = _isPlayerGrounded;
        }

        public void OnMovementSpeedChange(float newMovement)
        {
            movementSpeed = newMovement;
        }
        #endregion

        public bool TryMove()
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