using Cinemachine;
using Mirror;
using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Inputs;

namespace Warborn.Ingame.Characters.Player.PlayerModel.Movement
{
    /*
        * Basic Camera movement script, that rotates the player as well
    */
    public class PlayerCameraController : NetworkBehaviour
    {
        #region References
        [Header("Camera")]
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
        private CinemachineTransposer transposer;
        [Header("Player")]
        [SerializeField] private Transform playerTransform = null;
        #endregion

        #region Controls
        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }

        }
        [ClientCallback]
        private void OnEnable() => Controls.Enable();

        [ClientCallback]
        private void OnDisable() => Controls.Disable();
        #endregion

        public override void OnStartAuthority()
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            virtualCamera.gameObject.SetActive(true);
            enabled = true;
            Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }

        private void Look(Vector2 _lookAxis)
        {
            float followOffset = Mathf.Clamp(
                transposer.m_FollowOffset.y - (_lookAxis.y * cameraVelocity.y * Time.deltaTime),
                 maxFollowOffset.x,
                maxFollowOffset.y);

            transposer.m_FollowOffset.y = followOffset;

            playerTransform.Rotate(0f, _lookAxis.x * cameraVelocity.x * Time.deltaTime, 0f);
        }
    }
}



