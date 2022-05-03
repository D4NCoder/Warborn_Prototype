using UnityEngine;
using UnityEngine.UI;
using DuloGames.UI;
using Warborn.Ingame.Helpers;

namespace Warborn.Ingame.Characters.Player.PlayerGUI.Stats
{
    public class GUIStatsController : MonoBehaviour
    {
        #region References
        [SerializeField] private UIModalBox modalForInteractables;
        [SerializeField] private GameObject ProgressBarGO;

        [SerializeField] private Text movementSpeedText;
        [SerializeField] private Text attackDamageText;
        [SerializeField] private Text armorText;

        private IUIProgressBar progressBar;
        #endregion

        #region Initialization
        void Start()
        {
            progressBar = ProgressBarGO.GetComponent<IUIProgressBar>();
        }
        #endregion

        #region Health
        public void OnCurrentHealthChange(int _health)
        {
            float _maxHealth = 200f;
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


    }
}

