using Mirror;
using UnityEngine;
using Warborn.Ingame.Items.CraftingItems;

namespace Warborn.Ingame.Map.Core.Dropper
{
    public class ItemDropper : NetworkBehaviour
    {
        #region References

        #region Dropper init
        [Header("Dropper initialization")]
        [SerializeField] private ItemDropperGUI dropperGUI = null;
        [SerializeField] private Transform dropPlaceholder = null;
        [SerializeField] private GameObject particles = null;
        #endregion

        #region Dropper basic info
        [Header("Dropper basic information")]
        [SyncVar][SerializeField] private bool isActive;
        [SyncVar][SerializeField] private int dropCounter;
        [SerializeField] private CraftingItemSO drop = null;
        private GameObject dropGO = null;
        #endregion
        private float timer;

        #endregion

        #region Initialization, Reset and Activation
        public void Start()
        {
            if (isServer)
            {
                ResetCounterAndDrop();
            }
            if (isClient)
            {
                ActivateParticleSystem(false);
            }
        }

        [Client]
        public void ActivateDropper(bool _value)
        {
            CmdActivateDropper(_value);
        }

        [Command(requiresAuthority = false)]
        public void CmdActivateDropper(bool _value)
        {
            isActive = _value;
            RpcActivateParticleSystem(isActive);

            if (!isActive) { ResetCounterAndDrop(); }
        }

        [Server]
        public void ResetCounterAndDrop()
        {
            timer = 0f;
            dropCounter = 0;
            drop = null;
            CancelInvoke();
        }

        [Server]
        private void ResetCounter()
        {
            timer = 0f;
            dropCounter = 0;
            dropperGUI.RpcUpdateCounter(dropCounter);
        }

        [ClientRpc]
        public void RpcActivateParticleSystem(bool _value)
        {
            ActivateParticleSystem(_value);
        }

        [Client]
        public void ActivateParticleSystem(bool _value)
        {
            particles.SetActive(_value);
        }
        #endregion

        #region Dropping items
        [Server]
        private void Drop()
        {
            dropCounter++;
            if (dropCounter == 1)
            {
                SpawnItem();
            }
            RpcDrop();
        }

        [ClientRpc]
        private void RpcDrop()
        {
            dropperGUI.UpdateCounter(dropCounter);
        }
        #endregion

        #region Spawn drop

        [Server]
        private void SpawnItem()
        {
            if (drop == null) { return; }
            dropGO = Instantiate(drop.Prefab, dropPlaceholder.position, Quaternion.identity, dropPlaceholder);
            dropGO.GetComponent<DropableItem>().onPickupItem += OnPickupItem;
            dropGO.GetComponent<DropableItem>().ParentNetworkID = this.netIdentity;

            NetworkServer.Spawn(dropGO);
            RpcInstantiateDropGO(dropGO.GetComponent<NetworkIdentity>());
        }

        public void RpcInstantiateDropGO(NetworkIdentity _identity)
        {
            dropGO = _identity.gameObject;
        }

        private void OnPickupItem()
        {
            NetworkServer.Destroy(dropGO);
            ResetCounter();
        }


        #endregion

        #region Drop management
        [Server]
        public void AddItemToDrop(CraftingItemSO _item, float _duration)
        {
            drop = _item;
            CancelInvoke();
            InvokeRepeating(nameof(Drop), 0f, _duration);
        }
        #endregion
    }
}

