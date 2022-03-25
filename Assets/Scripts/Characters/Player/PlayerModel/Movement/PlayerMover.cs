using Mirror;
using UnityEngine;

namespace Warborn.Characters.Player.PlayerModel.Movement
{
    public class PlayerMover : NetworkBehaviour
    {
        #region  Editor variables
        [Header("References")]
        [SerializeField] private Rigidbody playerBody = null;
        [SerializeField] private Animator playerAnimator = null;
        [Header("Initial variables")]
        [SerializeField] private float movementSpeed = 7f;

        [SyncVar]
        [SerializeField] private bool isPlayerGrounded = false;
        #endregion
        private Vector2 playerMovementInput;

        public bool TryMove()
        {
            Vector3 _moveVector = transform.TransformDirection(new Vector3(playerMovementInput.x, 0f, playerMovementInput.y)) * movementSpeed;
            playerBody.velocity = new Vector3(_moveVector.x, playerBody.velocity.y, _moveVector.z);

            Vector2 _animatorVector = new Vector2(playerBody.velocity.normalized.z, playerBody.velocity.normalized.x);
            float _animatorSpeed = _animatorVector.magnitude;
            playerAnimator.SetFloat("forwardSpeed", _animatorSpeed);

            return true;
        }

        #region Events subscription methods
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
        #endregion
    }
}