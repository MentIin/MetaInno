using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic.Characters.Hands.Objects
{
    public class Box : Grabbable
    {
       [SerializeField] private BoxCollider _boxCollider;
       [SerializeField]private LayerMask _groundMask;

       private float _yVelocity = 0f;
       

       private void FixedUpdate()
        {
            if (isGrabbed)
            {
                _boxCollider.enabled = false;
                _yVelocity = 0f;
                return;
            }

            _boxCollider.enabled = true;
            
            if (CheckGrounded())
            {
                _yVelocity = 0f;
            }
            else
            {
                _yVelocity += -10f * Time.fixedDeltaTime;
            }
            
            
            transform.position += new Vector3(0f, _yVelocity * Time.fixedDeltaTime, 0f);
        }

        private bool CheckGrounded()
        {
            Vector3 center = _boxCollider.bounds.center;
            Vector3 extents = _boxCollider.bounds.extents;
            Vector3 direction = Vector3.down;
            float distance = 0.1f;

            RaycastHit[] hits = Physics.BoxCastAll(center, extents, direction, Quaternion.identity, distance, _groundMask);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != _boxCollider)
                {
                    return true;
                }
            }

            return false;
        }
    }
}