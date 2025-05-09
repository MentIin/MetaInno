using System;
using NUnit.Framework;
using UnityEngine;

namespace CodeBase.Logic.Camera.CameraLogic
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        public void SetTargetPosition(Vector3 targetPosition)
        {
            // Logic to set the camera's target position
            _targetPosition = targetPosition;
        }
        public void SetTargetLookAt(Vector3 targetLookAt)
        {
            // Logic to set the camera's target look at
            _targetRotation = Quaternion.LookRotation(targetLookAt - transform.position);
        }


        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, .1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, .1f);
        }
    }
}