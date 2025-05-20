using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands
{
    public class Grabbable : NetworkBehaviour
    {
        private NetworkTransform _networkTransform;
        protected bool isGrabbed;
        
        public bool IsGrabbed => isGrabbed;

        private void Awake()
        {
            if (!_networkTransform)
                _networkTransform = GetComponent<NetworkTransform>();
        }

        public void Grab(NetworkObject networkObject)
        {
            
            transform.SetParent(networkObject.transform);
            isGrabbed = true;
            GrabServerRpc(networkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GrabServerRpc(NetworkObject handNetObject)
        {
            transform.SetParent(handNetObject.transform);
            isGrabbed = true;
            GrabObserverRpc(handNetObject);
        }

        [ObserversRpc]
        private void GrabObserverRpc(NetworkObject handNetObject)
        {
            if (IsOwner) return;
            transform.SetParent(handNetObject?.transform);
            isGrabbed = true;
        }

        public void Ungrab()
        {
            transform.parent = null;
            isGrabbed = false;
            UngrabServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void UngrabServerRpc()
        {
            transform.parent = null;
            isGrabbed = false;
            UngrabObserverRpc();
        }

        [ObserversRpc]
        private void UngrabObserverRpc()
        {
            if (IsOwner) return;
            transform.parent = null;
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