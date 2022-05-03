using UnityEngine;
using Mirror;
using Warborn.Ingame.Map.Core.Dropper;
using Warborn.Ingame.Map.Core;
using Warborn.Ingame.Items.CraftingItems;

namespace Warborn.Ingame.Map.Forest
{
    public class ForestManager : NetworkBehaviour
    {
        #region References
        public NetworkIdentity ForestPillar;
        [SyncVar(hook = nameof(onForestPillarStateChanged))]
        [SerializeField] private bool isForestPillarActive = false;
        public bool IsForestPillarActive { get { return isForestPillarActive; } set { isForestPillarActive = value; } }

        [SyncVar]
        private int forestLevel = 0;

        [SerializeField] private string pathToDrop = "Scriptables/CraftableItems/";

        [SerializeField] private ItemDropper dropper1;
        [SerializeField] private ItemDropper dropper2;
        [SerializeField] private ItemDropper dropper3;
        #endregion

        #region Activate Pillar and Forest drops
        public void onForestPillarStateChanged(bool _oldValue, bool _newValue)
        {
            ForestPillar.GetComponent<InteractableObject>().Interact(_newValue);
            ActivateDroppers(_newValue);

            if (_newValue) { RequestResetLevel(); }
        }

        private void ActivateDroppers(bool _value)
        {
            dropper1.ActivateDropper(_value);
            dropper2.ActivateDropper(_value);
            dropper3.ActivateDropper(_value);
        }
        #endregion

        #region Level Up Forest and Droppers
        [ContextMenu("Level me Up")]
        public void RequestLevelUpForest()
        {
            LevelUpForest();
        }

        [Server]
        public void LevelUpForest()
        {
            forestLevel++;
            LevelUpDroppers();
        }

        [Server]
        private void LevelUpDroppers()
        {
            switch (forestLevel)
            {
                case 1:
                    AddItemToAllDroppers(LoadItem("Wood"), 10);
                    break;
                case 2:
                    AddItemToAllDroppers(LoadItem("Wood"), 5);
                    break;
                case 3:
                    AddItemToAllDroppers(LoadItem("Wood"), 3);
                    break;
                default:
                    ResetAllDroppers();
                    break;
            }
        }

        #endregion

        #region Droppers initialization
        private void AddItemToAllDroppers(CraftingItemSO _item, float _duration)
        {
            dropper1.AddItemToDrop(_item, _duration);
            dropper2.AddItemToDrop(_item, _duration);
            dropper3.AddItemToDrop(_item, _duration);
        }
        private CraftingItemSO LoadItem(string _nameOfDrop)
        {
            return (CraftingItemSO)Resources.Load(pathToDrop + _nameOfDrop);
        }
        #endregion

        #region Reset Level
        [Client]
        private void RequestResetLevel()
        {
            CmdResetLevel();
        }

        [Command(requiresAuthority = false)]
        public void CmdResetLevel()
        {
            forestLevel = 0;
            LevelUpForest();
        }

        [Server]
        private void ResetAllDroppers()
        {
            dropper1.ResetCounterAndDrop();
            dropper2.ResetCounterAndDrop();
            dropper3.ResetCounterAndDrop();
        }
        #endregion
    }
}

