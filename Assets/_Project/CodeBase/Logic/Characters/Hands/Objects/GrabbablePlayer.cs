using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands.Objects
{
    public class GrabbablePlayer : Grabbable
    {
        [SerializeField] private PlayerController _playerController;
        
        private void FixedUpdate()
        {

            _playerController.enabled =!isGrabbed;
        }
    }
}