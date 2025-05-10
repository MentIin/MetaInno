using System;
using CodeBase.Infrastructure.Services.Input;
using NUnit.Framework;
using UnityEngine;

namespace CodeBase.Logic.Camera.CameraLogic
{
    public class CameraController : MonoBehaviour
    {
        public Vector3 Offset = new Vector3(0f, 1.5f, -5f);
        
        private Transform _targetTransform;
        private IInputService _inputService;


        private Vector3 _realOffset;

        private float _distance;

        public void Construct(Transform transform1, IInputService inputService)
        {
            _distance = Offset.z;

            _realOffset = Offset;
            _realOffset.z = 0f;
            
            _targetTransform = transform1;
            _inputService = inputService;
        }
        
        private void LateUpdate()
        {
            return;
            if (_targetTransform == null) return;
            
            transform.position = Vector3.Lerp(transform.position,
                _targetTransform.position + _realOffset + new Vector3(_targetTransform.forward.x, 0f,  _targetTransform.forward.y) * _distance,
                .1f);
            //transform.rotation = Quaternion.Slerp(transform.rotation,     _targetTransform.rotation, .1f);
            
            transform.LookAt(_targetTransform.position + Vector3.up);
        }
    }
}