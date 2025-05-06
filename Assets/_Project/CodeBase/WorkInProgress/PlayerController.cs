using CodeBase.Infrastructure.Services.Input;
using CodeBase.Tools;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    private IInputService _inputService;
    private CharacterController _controller;

    [SerializeField] private List<CharacterBase> _characters;
    [SerializeField] private CharacterBase _currentCharacter;
    private int _currentIndexInList;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputService = new MockInputService();

        if (!_characters.Contains(_currentCharacter))
            Debug.LogWarning("Initial character not in list");
        else
            _currentIndexInList = _characters.IndexOf(_currentCharacter);

        ConstructCharacters();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
        {
            this.enabled = false;
            _currentCharacter.enabled = false;
            foreach (CharacterBase character in _characters)
            {
                character.enabled = false;
            }
            return;
        }

        HideCharacters();
    }

    private void Update()
    {
        if (_inputService.ActionKeyDown())
            StartActionOnCurrentCharacter();

        if (_inputService.ActionKeyUp())
            StopActionOnCurrentCharacter();

        if (_inputService.CharacterChangePressed())
            ChangeCharacter();
    }

    [ServerRpc]
    private void ChangeCharacter()
    {
        int nextIndex = _currentIndexInList == _characters.Count - 1 ? 0 : _currentIndexInList + 1;
        
        _currentCharacter.OnCharacterUnequipped();

        _currentCharacter = _characters[nextIndex];
        _currentIndexInList = nextIndex;
        _controller.ImportData(_currentCharacter.Data);

        _currentCharacter.OnCharacterEquipped();
    }

    private void FixedUpdate()
    {
        if (IsClientInitialized && IsOwner)
            MoveCurrentCharacter();
    }

    [ServerRpc]
    private void MoveCurrentCharacter() =>
        _currentCharacter.Move(_inputService.GetAxis());

    [ServerRpc]
    private void StartActionOnCurrentCharacter() =>
        _currentCharacter.ActionStart();

    [ServerRpc]
    private void StopActionOnCurrentCharacter() =>
        _currentCharacter.ActionStop();

    private void ConstructCharacters()
    {
        foreach (CharacterBase character in _characters)
        {
            character.Construct(_controller, transform);
        }
    }

    [ServerRpc]
    private void HideCharacters()
    {
        foreach (CharacterBase character in _characters)
            character.OnCharacterUnequipped();

        _currentCharacter.OnCharacterEquipped();
    }
}