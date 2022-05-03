using UnityEngine;

namespace Warborn.Ingame.Map.Core.InteractableObjects
{
    public class StonePillar : InteractableObject
    {
        void Start()
        {
            Name = "Stone Pillar";
        }
        public GameObject StonePillarGO;
        public override void Interact(bool _value)
        {
            Runestone_Controller _runestoneCtr = StonePillarGO.GetComponent<Runestone_Controller>();
            isActive = _value;
            _runestoneCtr.ToggleRuneStone(isActive);

        }

        public override void Interact()
        {
            Runestone_Controller _runestoneCtr = StonePillarGO.GetComponent<Runestone_Controller>();
            isActive = !isActive;
            _runestoneCtr.ToggleRuneStone(isActive);
        }
    }
}

