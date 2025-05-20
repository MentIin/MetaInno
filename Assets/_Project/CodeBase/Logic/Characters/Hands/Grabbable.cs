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
        
        
        private Vector3 _grabLocalPosition = Vector3.zero;
        private Vector3 _grabLocalRotation = Vector3.zero;
        

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
            //isGrabbed = true;
            
            //_grabLocalPosition = handNetObject.transform.InverseTransformPoint(transform.position);
            _grabLocalPosition = transform.localPosition;
            
            GrabServerRpc(handNetObject, _grabLocalPosition);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GrabServerRpc(NetworkObject handNetObject, Vector3 grabLocalPosition)
        {
            // Только сервер меняет родителя
            _grabLocalPosition = grabLocalPosition;
            
            
            
            transform.SetParent(handNetObject.transform);
            
            transform.localPosition = grabLocalPosition;
            isGrabbed = true;
            
            // Форсированная синхронизация
            _networkTransform.ForceSend();
            GrabObserverRpc(handNetObject, _grabLocalPosition);
        }

        [ObserversRpc]
        private void GrabObserverRpc(NetworkObject handNetObject, Vector3 grabLocalPosition)
        {
            // Все клиенты получают обновление
            transform.SetParent(handNetObject?.transform);
            transform.localPosition = grabLocalPosition;
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