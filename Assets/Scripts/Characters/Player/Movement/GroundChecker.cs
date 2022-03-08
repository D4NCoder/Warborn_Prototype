using UnityEngine;

namespace Warborn.Characters.Player.Movement
{
    public class GroundChecker : MonoBehaviour
    {
        #region Variables
        [SerializeField] private bool isPlayerGrounded;
        public bool GetPlayerGrounded
        {
            get { return isPlayerGrounded; }
        }
        #endregion

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6)
            {
                isPlayerGrounded = true;
            }
        }
        void OnCollisionExit(Collision other)
        {
            isPlayerGrounded = false;
        }
    }
}