using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerGUI.Stats;

namespace Warborn.Ingame.Map.Core.InteractableObjects
{
    public class FountainOfUndying : InteractableObject
    {
        public override void Interact()
        {
            throw new System.NotImplementedException();
        }

        public override void Interact(bool _value)
        {
            throw new System.NotImplementedException();
        }

        public override void Interact(GameObject _playerGUI)
        {
            GUIStatsController _statsController = _playerGUI.GetComponent<GUIStatsController>();
            _statsController.ShowFountainsGUI(true);
        }
    }

}