using UnityEngine;
using UnityEngine.UI;
using DuloGames.UI;
using Warborn.Ingame.Helpers;
using Warborn.Ingame.Characters.Player.PlayerGUI.DamagableGUI;
using Warborn.Ingame.Settings;

namespace Warborn.Ingame.Characters.Player.PlayerGUI.Stats
{
    public class GUIStatsController : MonoBehaviour
    {
        #region References
        [SerializeField] private UIModalBox modalForInteractables;
        [SerializeField] private GameObject damagableTargetGUI;

        [SerializeField] private GameObject fountainOfUndyingGO;
        [SerializeField] private FountainOfUndyingGUI fountainOfUndyingGUI;
        public FountainOfUndyingGUI FountainOfUndyingGUI => fountainOfUndyingGUI;

        [SerializeField] private GameObject progressBarGO;
        private IUIProgressBar progressBar;

        [SerializeField] private Text healthText;
        [SerializeField] private Text movementSpeedText;
        [SerializeField] private Text attackDamageText;
        [SerializeField] private Text armorText;
        #endregion

        #region Initialization
        private void Awake()
        {
            progressBar = progressBarGO.GetComponent<IUIProgressBar>();
        }
        #endregion

        #region Stats
        public void OnCurrentHealthChange(int _health, int _maxHealth)
        {
            healthText.text = _health + " / " + _maxHealth;
            progressBar.fillAmount = ((float)_health).Remap(0, _maxHealth, 0, 1);
        }
        #endregion

        #region Modal popup for interactables
        public void ShowInteractionText(string _text)
        {
            modalForInteractables.SetText1(_text);
            modalForInteractables.Show();
        }
        public void HideInteractionText()
        {
            modalForInteractables.Close();
        }
        #endregion

        #region Damagable interaction

        public void ShowDamagableGUI(bool _enemy, int _maxHealth, int _currentHealth, string _name)
        {
            damagableTargetGUI.SetActive(true);
            UpdateDamagableGUI(_enemy, _maxHealth, _currentHealth, _name);
        }

        public void UpdateDamagableGUI(bool _enemy, int _maxHealth, int _currentHealth, string _name)
        {
            damagableTargetGUI.GetComponent<DamagableGUIHandler>().UpdateGUI(_enemy, _maxHealth, _currentHealth, _name);
        }

        public void UpdateHealthOfStatue(int _newHealth)
        {
            damagableTargetGUI.GetComponent<DamagableGUIHandler>().UpdateHealthGUI(_newHealth);
        }

        public void HideDamagableGUI()
        {
            damagableTargetGUI.SetActive(false);
        }

        #endregion

        #region Fountain of Undying
        public void ShowFountainsGUI(bool _value)
        {
            fountainOfUndyingGO.SetActive(_value);
            ActivateCursor(_value);
        }
        #endregion

        private void ActivateCursor(bool _value)
        {
            if (_value) { CursorSettings.UnlockCursor(); }
            else { CursorSettings.LockCursor(); }
        }
    }
}

