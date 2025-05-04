using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected CharacterController _controller;
    [SerializeField] protected Transform _visuals;
    protected Transform _parent;

    [SerializeField] private CharacterData _data;

    public CharacterData Data => _data;
    
    public void Construct(CharacterController controller, Transform parent)
    {
        _controller = controller;
        _parent = parent;
    }

    public abstract void Move(Vector2 inputAxis);
    public abstract void ActionStart();
    public abstract void ActionStop();

    public virtual void OnCharacterEquipped()
    {
        gameObject.SetActive(true);
    }
    public virtual void OnCharacterUnequipped()
    {
        gameObject.SetActive(false);
    }
}
