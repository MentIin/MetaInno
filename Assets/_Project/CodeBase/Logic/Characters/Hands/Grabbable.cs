using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands
{
    public class Grabbable : NetworkBehaviour
    {
        protected bool isGrabbed;
        
        
        
        
        [ServerRpc(RequireOwnership = false)]
        private void GrabServerRpc(Transform hand)
        {
            //transform.SetParent(hand);
            isGrabbed = true;
            GrabObserverRpc(hand);
        }
        [ObserversRpc(ExcludeOwner = true)]
        private void GrabObserverRpc(Transform hand)
        {
            //transform.SetParent(hand);
            isGrabbed = true;
        }
        public void Grab(Transform hand//, NetworkConnection owNetworkConnection
        )
        {
            //base.GiveOwnership(owNetworkConnection);
            
            transform.SetParent(hand);
            isGrabbed = true;
            
            GrabServerRpc(hand);
        }
        
        
        
        
        public void Ungrab()
        {
            isGrabbed = false;
            transform.parent = null;
            UngrabServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void UngrabServerRpc()
        {
            isGrabbed = false;
            //transform.parent = null;
            UngrabObserverRpc();
        }
        [ObserversRpc(ExcludeOwner = true)]
        private void UngrabObserverRpc()
        {
            isGrabbed = false;
            //transform.parent = null;
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