using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class StandaloneInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        private ICoroutineRunner _coroutineRunner;

        public event Action<int> ModulePressed;

        public StandaloneInputService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Initialize()
        {
            _coroutineRunner.StartCoroutine(Check());
        }

        public Vector2 GetAxis()
        {
            //return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical)).normalized;
            throw new NotImplementedException();
        }
        
        

        public void ClearInput()
        {
            
        }

        private IEnumerator Check()
        {
            while (true)
            {
                
                
                yield return null;
            }
        }
    }
}