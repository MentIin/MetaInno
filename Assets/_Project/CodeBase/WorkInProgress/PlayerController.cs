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
    [SerializeField] private bool _clientAuth = true;
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
        if (_clientAuth)
        {
            if (_inputService.ActionKeyDown())
                _currentCharacter.ActionStart();

            if (_inputService.ActionKeyUp())
                _currentCharacter.ActionStop();

            if (_inputService.CharacterChangePressed())
                ChangeCharacter();
        } 
        else
        {
            if (_inputService.ActionKeyDown())
                StartActionOnCurrentCharacter();

            if (_inputService.ActionKeyUp())
                StopActionOnCurrentCharacter();

            if (_inputService.CharacterChangePressed())
                ChangeCharacterRPC();
        }
    }

    private void ChangeCharacter()
    {
        int nextIndex = _currentIndexInList == _characters.Count - 1 ? 0 : _currentIndexInList + 1;
        
        _currentCharacter.OnCharacterUnequipped();

        _currentCharacter = _characters[nextIndex];
        _currentIndexInList = nextIndex;
        _controller.ImportData(_currentCharacter.Data);

        _currentCharacter.OnCharacterEquipped();
    }

    [ServerRpc]
    private void ChangeCharacterRPC()
    {
        ChangeCharacter();
    }

    private void FixedUpdate()
    {
        if (!IsClientInitialized)
            return;

        if (_clientAuth)
            _currentCharacter.Move(_inputService.GetAxis());
        else
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