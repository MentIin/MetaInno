using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class StandaloneInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public StandaloneInputService()
        {
            
        }

        public void Initialize()
        {
            
        }

        public Vector2 GetAxis()
        {
            //return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical)).normalized;
            throw new NotImplementedException();
        }
        
        

        public void ClearInput()
        {
            
        }

        public bool ActionKeyDown()
        {
            throw new NotImplementedException();
        }

        public bool ActionKeyUp()
        {
            throw new NotImplementedException();
        }

        bool IInputService.CharacterChangePressed()
        {
            throw new NotImplementedException();
        }
    }
}