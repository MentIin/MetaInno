using UnityEngine;

public class InnikCharacter : CharacterBase
{
    public override void ActionStart()
    {
        _controller.Move(Vector3.up);
    }

    public override void ActionStop()
    {
        
    }

    public override void Move(Vector2 inputAxis)
    {
        Vector3 direction = _parent.forward * inputAxis.y + _parent.right * inputAxis.x;
        _controller.Move(direction * Time.fixedDeltaTime);
    }
}
