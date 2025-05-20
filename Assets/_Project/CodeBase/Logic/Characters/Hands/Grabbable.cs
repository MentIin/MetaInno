using FishNet.Object;
using FishNet.Component.Transforming;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands
{
    public class Grabbable : NetworkBehaviour
    {
        [SerializeField] private NetworkTransform _networkTransform;
        protected bool isGrabbed;
        
        public bool IsGrabbed => isGrabbed;

        private void Awake()
        {
            if (!_networkTransform)
                _networkTransform = GetComponent<NetworkTransform>();
        }

        public void Grab(NetworkObject handNetObject)
        {
            if (handNetObject == null) return;

            // Локальное предсказание
            transform.SetParent(handNetObject.transform);
            isGrabbed = true;
            
            GrabServerRpc(handNetObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GrabServerRpc(NetworkObject handNetObject)
        {
            // Только сервер меняет родителя
            transform.SetParent(handNetObject.transform);
            isGrabbed = true;
            
            // Форсированная синхронизация
            _networkTransform.ForceSend();
            GrabObserverRpc(handNetObject);
        }

        [ObserversRpc]
        private void GrabObserverRpc(NetworkObject handNetObject)
        {
            // Все клиенты получают обновление
            transform.SetParent(handNetObject?.transform);
            isGrabbed = true;
        }

        public void Ungrab()
        {
            transform.SetParent(null);
            isGrabbed = false;
            UngrabServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void UngrabServerRpc()
        {
            transform.SetParent(null);
            isGrabbed = false;
            _networkTransform.ForceSend();
            UngrabObserverRpc();
        }

        [ObserversRpc]
        private void UngrabObserverRpc()
        {
            transform.SetParent(null);
            isGrabbed = false;
        }
        
        public Vector3 GetGrabPoint(Transform hand1)
        {
            Vector3 pos;
            pos = hand1.position - transform.position;
            pos = pos.normalized * 0.6f;
            pos = transform.position + pos;
            pos.y = transform.position.y - 0.1f;
            return pos;
        }
    }
}