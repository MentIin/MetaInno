using UnityEngine;

public class RoverCharacter : CharacterBase
{
    private float gravity=-9.81f;
    private float _yVelocity=0f;
    
    
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
