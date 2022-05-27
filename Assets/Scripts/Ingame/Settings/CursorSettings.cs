using UnityEngine;

namespace Warborn.Ingame.Settings
{
    public class CursorSettings : MonoBehaviour
    {
        public static void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public static bool IsCursorLocked() { return !Cursor.visible; }
    }
}

