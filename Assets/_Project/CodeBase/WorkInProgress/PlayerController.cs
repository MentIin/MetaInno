using CodeBase.Infrastructure.Services.Input;
using CodeBase.Tools;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
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
    }

    private void Start()
    {
        ConstructCharacters();
        HideCharacters();
    }

    private void Update()
    {
        if (_inputService.ActionKeyDown())
            _currentCharacter.ActionStart();

        if (_inputService.ActionKeyUp())
            _currentCharacter.ActionStop();

        if (_inputService.CharacterChangePressed())
            ChangeCharacter();
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

    private void FixedUpdate()
    {
        _currentCharacter.Move(_inputService.GetAxis());
    }

    private void ConstructCharacters()
    {
        foreach (CharacterBase character in _characters)
        {
            character.Construct(_controller, transform);
        }
    }

    private void HideCharacters()
    {
        foreach (CharacterBase character in _characters)
            character.OnCharacterUnequipped();

        _currentCharacter.OnCharacterEquipped();
    }
}