using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.Input;
using System;
using UnityEngine;

public class MockInputService : IInputService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const KeyCode SpecialActionKey = KeyCode.Space;
    private const KeyCode CharacterChangeKey = KeyCode.R;

    public MockInputService()
    {
        
    }

    public void Initialize()
    {
        
    }

    public Vector2 GetAxis()
    {
        return new Vector2(SimpleInput.GetAxisRaw(Horizontal), SimpleInput.GetAxisRaw(Vertical)).normalized;
    }

    public bool ActionKeyDown()
    {
        return SimpleInput.GetKeyDown(SpecialActionKey);
    }

    public bool ActionKeyUp()
    {
        return SimpleInput.GetKeyUp(SpecialActionKey);
    }

    bool IInputService.CharacterChangePressed()
    {
        return SimpleInput.GetKeyDown(CharacterChangeKey);
    }

    public void ClearInput()
    {

    }
}
