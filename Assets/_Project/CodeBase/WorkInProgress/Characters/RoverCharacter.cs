using UnityEngine;

public class RoverCharacter : CharacterBase
{
    public override void ActionStart()
    {
        throw new System.NotImplementedException();
    }

    public override void ActionStop()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(Vector2 inputAxis)
    {
        Quaternion rotation = Quaternion.AngleAxis(inputAxis.x, transform.up);
        _parent.rotation *= rotation;

        Vector3 direction = _parent.forward * inputAxis.y;
        _controller.Move(direction  * Time.fixedDeltaTime);
    }
}
