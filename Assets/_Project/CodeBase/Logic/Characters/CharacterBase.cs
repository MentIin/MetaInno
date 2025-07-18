using CodeBase.Logic.Characters;
using FishNet.Object;
using UnityEngine;

public abstract class CharacterBase : NetworkBehaviour
{
    protected CharacterController _controller;
    [SerializeField] protected Transform _visuals;
    protected Transform _parent;

    [SerializeField] private CharacterData _data;
    protected ExternalForceController _externalForceController;

    public CharacterData Data => _data;
    
    public void Construct(CharacterController controller, Transform parent, ExternalForceController externalForceController)
    {
        _controller = controller;
        _parent = parent;
        _externalForceController = externalForceController;
        
        Initialize();
    }

    public virtual void Initialize(){}

    public abstract void Move(Vector2 _inputAxis);
    public abstract void ActionStart();
    public abstract void ActionStop();
    
    public virtual void OnCharacterEquipped()
    {
        gameObject.SetActive(true);
        return;
        foreach (var VARIABLE in GetComponentsInChildren<MeshRenderer>(true))
        {
            VARIABLE.enabled = true;
        }
    }
    

    
    

    public virtual void OnCharacterUnequipped()
    {
        gameObject.SetActive(false);
        return;
        foreach (var VARIABLE in GetComponentsInChildren<MeshRenderer>(true))
        {
            VARIABLE.enabled = false;
        }
    }

    public abstract void SecondaryActionStart();
    public abstract void SecondaryActionStop();
}
