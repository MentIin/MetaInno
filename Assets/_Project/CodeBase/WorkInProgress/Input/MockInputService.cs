using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.Input;
using System;
using UnityEngine;

public class MockInputService : IInputService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const KeyCode SpecialActionKey = KeyCode.Space;
    private const KeyCode CharacterChangeKey = KeyCode.E;

    public MockInputService()
    {
        
    }

    public void Initialize()
    {
        
    }

    public Vector2 GetAxis()
    {
        return new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)).normalized;
    }

    public bool ActionKeyDown()
    {
        return Input.GetKeyDown(SpecialActionKey);
    }

    public bool ActionKeyUp()
    {
        return Input.GetKeyUp(SpecialActionKey);
    }

    bool IInputService.CharacterChangePressed()
    {
        return Input.GetKeyDown(CharacterChangeKey);
    }

    public void ClearInput()
    {

    }
}
