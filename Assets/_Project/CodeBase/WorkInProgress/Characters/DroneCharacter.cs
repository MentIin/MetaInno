using UnityEngine;

public class DroneCharacter : CharacterBase
{
    private float _verticalVelocity;
    private bool _tryingToFly = false;

    public override void ActionStart()
    {
        _tryingToFly = true;
    }

    public override void ActionStop()
    {
        _tryingToFly = false;
    }

    public override void Move(Vector2 inputAxis)
    {
        Vector3 direction = _parent.forward * inputAxis.y + _parent.right * inputAxis.x;
        Vector3 desiredHorizontalMovement = direction * 3f;

        if (_controller.isGrounded)
        {
            desiredHorizontalMovement = Vector3.zero;
            if (!_tryingToFly)
                _verticalVelocity = -0.1f;
        }
        else
        {
            _verticalVelocity += -6f * Time.deltaTime;
        }

        if (_tryingToFly)
            _verticalVelocity += 12f * Time.deltaTime;

        Vector3 upVelocity = Vector3.up * _verticalVelocity;
        _controller.Move((desiredHorizontalMovement + upVelocity) * Time.deltaTime);
    }
}
