using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands.Objects
{
    public class GrabbablePlayer : Grabbable
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CharacterController _characterController;
        
        private void FixedUpdate()
        {

            _characterController.enabled = !isGrabbed;
            _playerController.enabled =!isGrabbed;
        }
    }
}