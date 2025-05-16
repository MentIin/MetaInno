using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands
{
    public class Grabbable : NetworkBehaviour
    {
        public void Grab(Transform hand)
        {
            transform.SetParent(hand);
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

        public void Ungrab()
        {
            transform.parent = null;
        }
    }
}