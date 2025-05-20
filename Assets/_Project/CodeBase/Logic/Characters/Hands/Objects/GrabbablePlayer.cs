using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands.Objects
{
    public class GrabbablePlayer : Grabbable
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CharacterController _characterController;


        private Vector3 _cameraPos;
        private Vector3 _playerPos;
        private void FixedUpdate()
        {

            _characterController.enabled = !isGrabbed;
            _playerController.enabled =!isGrabbed;


            if (isGrabbed)
            {
                UnityEngine.Camera.main.transform.position = _cameraPos + transform.position - _playerPos;
            }else
            {
                _cameraPos = UnityEngine.Camera.main.transform.position;
                _playerPos = transform.position;
            }
        }
    }
}