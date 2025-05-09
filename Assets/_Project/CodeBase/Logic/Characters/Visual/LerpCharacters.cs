using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Visual
{
    public class LerpCharacters : MonoBehaviour
    {
        private Transform _parent;

        private void Start()
        {
            if (transform.parent == null)
            {
                Debug.LogWarning("LerpCharacters: No parent found. Destroying this object.");
                Destroy(this);
                return;
            }

            _parent = transform.parent;
            
            transform.parent = null;
        }


        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _parent.position, 0.9f);
            transform.rotation = Quaternion.Slerp(transform.rotation, _parent.rotation, 0.9f);
        }
    }
}