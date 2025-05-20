using System;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Tools;
using FishNet.Object;
using System.Collections.Generic;
using CodeBase.Logic.Camera.CameraLogic;
using CodeBase.Logic.Characters;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private ExternalForceController _externalForceController;
    [SerializeField] private List<CharacterBase> _characters;
    [SerializeField] private CharacterBase _currentCharacter;
    
    
    
    
    private IInputService _inputService;
    private CharacterController _controller;
    
    [SerializeField] private bool _clientAuth = true;
    private int _currentIndexInList;
    private Vector2 cameraRotation;

    
    
    public CharacterBase CurrentCharacter { get => _currentCharacter;}
    
    public Action CurrentCharacterChanged;
    
    

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputService = new MockInputService();

        ConstructCharacters();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!_characters.Contains(_currentCharacter))
            Debug.LogWarning("Initial character not in list");
        else
            _currentIndexInList = _characters.IndexOf(_currentCharacter);

        if (base.IsOwner)
        {
            SetCurrentCharacterIndexServer(0);

            Camera.main.gameObject.GetComponent<CameraController>().Construct(transform, _inputService);
        }
        
    }

    

    private void Update()
    {
        if (!base.IsOwner) return;
        
        if (_clientAuth)
        {
            if (_inputService.ActionKeyDown())
                _currentCharacter.ActionStart();

            if (_inputService.ActionKeyUp())
                _currentCharacter.ActionStop();
        } 
        else
        {
            if (_inputService.ActionKeyDown())
                StartActionOnCurrentCharacter();

            if (_inputService.ActionKeyUp())
                StopActionOnCurrentCharacter();
        }
        
        if (_inputService.SecondaryActionKeyDown())
        {
            if (_clientAuth)
                _currentCharacter.SecondaryActionStart();
        }else if (_inputService.SecondaryActionKeyUp())
        {
            if (_clientAuth)
                _currentCharacter.SecondaryActionStop();
        }
        
        
        if (_inputService.CharacterChangePressed())
            ChangeCharacterRPC();
        
        
    }
    private void FixedUpdate()
    {
        if (!IsClientInitialized)
            return;
        if (!base.IsOwner) return;
        
        
        
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0; // Ignore vertical component
        cameraRight.y = 0;   // Ignore vertical component

        cameraForward.Normalize();
        cameraRight.Normalize();

        if (_currentCharacter.Data.moveAccordingToCamera)
        {
            Vector3 direction = cameraForward * _inputService.GetAxis().y + cameraRight * _inputService.GetAxis().x;
        
            //MoveCurrent(new Vector2(direction.x, direction.z));

            if (_clientAuth)
            {
                MoveCurrent(new Vector2(direction.x, direction.z));
            }
            else
            {
                MoveCurrentCharacterRPC(new Vector2(direction.x, direction.z));
            }
            
            
        }
        else
        {
            if (_clientAuth)
            {
                MoveCurrent(_inputService.GetAxis());
            }
            else
            {
                MoveCurrentCharacterRPC(_inputService.GetAxis());
            }
        }
    }

    private void LateUpdate()
    {
        if (IsOwner) MoveCamera(_inputService.GetAxis());
    }


    private void MoveCamera(Vector2 inputAxis)
    {
        if (inputAxis.sqrMagnitude != 0)
        {
            cameraRotation = Vector2.Lerp(cameraRotation, new Vector2(transform.forward.x, transform.forward.z), 0.04f);
            cameraRotation = cameraRotation.normalized;
        }
        Camera.main.transform.position = (_currentCharacter.transform.position + new Vector3(0, 3, 0) +
                                                                       new Vector3(cameraRotation.x, 0f,  cameraRotation.y) * -5);
        
        Camera.main.transform.LookAt(_currentCharacter.transform.position + new Vector3(0, 1.5f, 0));
    }
    
    private void ChangeCharacter()
    {
        int nextIndex = _currentIndexInList == _characters.Count - 1 ? 0 : _currentIndexInList + 1;
        
        _currentCharacter.OnCharacterUnequipped();

        _currentCharacter = _characters[nextIndex];
        
        // I think extension method is unnesasery
        _controller.ImportData(_currentCharacter.Data);

        _currentCharacter.OnCharacterEquipped();
        
        
        
        _currentIndexInList = nextIndex;
        SetCurrentCharacterIndexObserversRpc(_currentIndexInList);
    }

    
    
    [ServerRpc]
    private void ChangeCharacterRPC()
    {
        ChangeCharacter();
    }

    

    [ServerRpc(RequireOwnership = true)]
    private void MoveCurrentCharacterRPC(Vector2 inputAxis)
    {
        MoveCurrent(inputAxis);
    }
    
    
    [ServerRpc]
    private void SetCurrentCharacterIndexServer(int index)
    {
        SetCurrentCharacterIndex(index);
    }

    private void SetCurrentCharacterIndex(int index)
    {
        _currentIndexInList = index;
        SetCurrentCharacterIndexObserversRpc(index);
    }

    [ObserversRpc(BufferLast = true)]
    private void SetCurrentCharacterIndexObserversRpc(int i)
    {
        
        CurrentCharacterChanged?.Invoke();
        
        _currentIndexInList = i;
        _currentCharacter = _characters[i];
        
        
        int index = 0;
        foreach (var VARIABLE in _characters)
        {
            if (index == _currentIndexInList)
            {
                VARIABLE.OnCharacterEquipped();
            }
            else
            {
                VARIABLE.OnCharacterUnequipped();
            }
            
            index++;
        }
        
    }
    
    

    private void MoveCurrent(Vector2 inputAxis)
    {
        _currentCharacter.Move(inputAxis);
    }


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
            character.Construct(_controller, transform, _externalForceController);
        }
    }

    [ServerRpc]
    private void HideCharactersServerRpc()
    {
        HideCharacters();
    }

    private void HideCharacters()
    {
        foreach (CharacterBase character in _characters)
            character.OnCharacterUnequipped();

        _currentCharacter.OnCharacterEquipped();
    }
}