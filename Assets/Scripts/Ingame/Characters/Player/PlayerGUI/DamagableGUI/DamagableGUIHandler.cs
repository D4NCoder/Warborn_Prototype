using DuloGames.UI;
using UnityEngine;
using UnityEngine.UI;
using Warborn.Ingame.Helpers;

namespace Warborn.Ingame.Characters.Player.PlayerGUI.DamagableGUI
{
    public class DamagableGUIHandler : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text healthText;
        [SerializeField] private GameObject progressBarGO;

        [SerializeField] private Image dragonBorderImage;
        [SerializeField] private Image skullImage;
        [SerializeField] private Image skullBorderImage;
        [SerializeField] private Image circleBorderImage;

        private int maxHealth;
        public void UpdateGUI(bool _enemy, int _maxHealth, int _currentHealth, string _name)
        {
            maxHealth = _maxHealth;
            nameText.text = _name;

            UpdateColorOfGUI(_enemy);
            UpdateHealthGUI(_currentHealth);
        }

        private void UpdateColorOfGUI(bool _enemy)
        {
            Color32 colorTheme;

            if (_enemy) { colorTheme = new Color32(196, 66, 51, 255); }
            else { colorTheme = new Color32(253, 231, 76, 255); }

            dragonBorderImage.color = colorTheme;
            skullImage.color = colorTheme;
            skullBorderImage.color = colorTheme;
            circleBorderImage.color = colorTheme;
        }

        public void UpdateHealthGUI(int _newHealth)
        {
            healthText.text = _newHealth + " / " + maxHealth;
            float _remapedHealth = ((float)_newHealth).Remap(0f, (float)maxHealth, 0f, 1f);

            progressBarGO.GetComponent<IUIProgressBar>().fillAmount = _remapedHealth;
        }
    }
}

