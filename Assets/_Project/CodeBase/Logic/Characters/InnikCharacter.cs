using UnityEngine;
using UnityEngine.Serialization;

public class InnikCharacter : CharacterBase
{
    private float gravity=-15f;
    private float _yVelocity=0f;
    
    private float _jumpBuffer = 0.0f;
    private float _cayoutTime = 0.0f;
    private float _speed=4;


    public override void ActionStart()
    {
        _jumpBuffer = 0.3f;
        
    }

    public override void ActionStop()
    {
        if (_yVelocity > 0)
        {
            _yVelocity *= 0.5f;
        }
    }

    public override void Move(Vector2 _inputAxis)
    {
        JumpUpdate();


        Vector3 direction = Vector3.forward * _inputAxis.y + Vector3.right * _inputAxis.x;
        
        _controller.Move(direction * Time.fixedDeltaTime * _speed + Vector3.up * _yVelocity * Time.fixedDeltaTime);

        if (_inputAxis.sqrMagnitude != 0)
        {
            _controller.transform.rotation = Quaternion.LookRotation(direction);
        }
        
        
        
        if (_controller.isGrounded)
        {
            _yVelocity = 0f;
            _cayoutTime = 0.3f;
        }
        else
        {
            _yVelocity += gravity * Time.fixedDeltaTime;
        }
    }

    private void JumpUpdate()
    {
        _jumpBuffer -= Time.fixedDeltaTime;
        _cayoutTime -= Time.fixedDeltaTime;
        if (_jumpBuffer > 0)
        {
            if (_cayoutTime > 0)
            {
                _yVelocity = 11f;
                _jumpBuffer = 0f;
            }
        }
    }
}
