using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        event Action<int> ModulePressed;
        Vector2 GetAxis();
        void ClearInput();
        void Initialize();
    }
}