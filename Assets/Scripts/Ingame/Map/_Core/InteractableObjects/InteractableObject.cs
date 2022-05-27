using UnityEngine;

namespace Warborn.Ingame.Map.Core
{
    public abstract class InteractableObject : MonoBehaviour
    {
        public string Name;
        public bool isActive;
        public abstract void Interact();
        public abstract void Interact(bool _value);
        public abstract void Interact(GameObject _PlayerGUI);
    }

}
