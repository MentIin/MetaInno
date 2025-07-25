﻿using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Visual
{
    public class LerpAndDetachFromParent : MonoBehaviour
    {
        public bool SyncRotation = true;
        public bool SyncPosition = true;
        private Transform _parent;

        private void Start()
        {
            if (transform.parent == null)
            {
                Debug.LogWarning("LerpAndDetachFromParent: No parent found. Destroying this object.");
                Destroy(this.gameObject);
                return;
            }

            _parent = transform.parent;
            
            transform.parent = null;
        }


        private void Update()
        {
            if (transform == null)
            {
                Destroy(this.gameObject);
                return;
            }

            if (!_parent.gameObject.activeInHierarchy)
            {
                transform.parent = _parent;
            }
            else
            {
                transform.parent = null;
            }
            
            
            if (SyncPosition) transform.position = Vector3.Lerp(transform.position, _parent.position, 0.3f);
            
            if (SyncRotation) transform.rotation = Quaternion.Slerp(transform.rotation, _parent.rotation, 0.3f);
        }
    }
}