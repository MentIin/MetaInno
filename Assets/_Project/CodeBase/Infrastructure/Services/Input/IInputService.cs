using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService : IService
    {

        Vector2 GetAxis();
        bool ActionKeyDown();
        bool ActionKeyUp();
        bool CharacterChangePressed();

        void ClearInput();
        void Initialize();
    }
}