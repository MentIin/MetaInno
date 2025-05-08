using UnityEngine;
using UnityEngine.UIElements;

public class RoverCharacter : CharacterBase
{
    private float gravity=-12f;
    private float _yVelocity=0f;

    private float _currentMoveSpeed = 0f;
    private float _moveSpeed = 11f;

    private bool _tryingDrift;
    private bool _drift;
    
    private Vector3 _driftVelocity;
    private float _rotationSpeed = 60;
    
    private Vector2 driftAxis;
    
    private float _driftBoost=0f;
    

    public override void ActionStart()
    {
        _tryingDrift = true;
    }

    public override void ActionStop()
    {
        _tryingDrift = false;
    }

    public override void Move(Vector2 inputAxis)
    {
        if (!Mathf.Approximately(Mathf.Sign(inputAxis.x), Mathf.Sign(driftAxis.x)))
        {
            _drift = false;
        }
        
        if (inputAxis.y < 0)
        {
            inputAxis.x = -inputAxis.x;
            if (Mathf.Abs(inputAxis.x) < 0.1f)
            {
                inputAxis.x = 0;
            }
        }
        
        ReduceCurrentSpeed(inputAxis);
        UpdateGravity();

        if (_tryingDrift && !_drift && inputAxis.x != 0)
        {
            _drift = true;
            driftAxis = inputAxis; 
            _driftBoost = 1f;
        }
        else if (!_tryingDrift && _drift)
        {
            _drift = false;
            _driftVelocity = Vector3.zero;
        }

        if (_drift)
        {
            _driftBoost += Time.fixedDeltaTime*2;
            _driftVelocity = -_controller.transform.right * Mathf.Sign(inputAxis.x) * _driftBoost*2;
        }
        else
        {
            _driftVelocity = Vector3.zero;
            _driftBoost = 0;
        }
        
        

        Vector3 currentRotationVector = _controller.transform.forward;
        
        _currentMoveSpeed = Mathf.Clamp((_currentMoveSpeed + inputAxis.y*Time.fixedDeltaTime * 7) ,-_moveSpeed, _moveSpeed);
        Vector3 direction = currentRotationVector.normalized * Time.fixedDeltaTime * (_currentMoveSpeed + _driftBoost);
        direction += Vector3.up * _yVelocity * Time.fixedDeltaTime;
        
        
        float k = _drift ? 1.5f : 1;
        _controller.transform.Rotate(0f,  inputAxis.x* _rotationSpeed * Time.fixedDeltaTime * k, 0f);
        _controller.Move(direction + _driftVelocity * Time.fixedDeltaTime);
        
        _driftVelocity *= 0.9f;
    }

    private void UpdateGravity()
    {
        _yVelocity += gravity * Time.fixedDeltaTime;
        if (_controller.isGrounded) _yVelocity = 0;
    }

    private void ReduceCurrentSpeed(Vector2 inputAxis)
    {
        float desealerationSpeed = 3f;
        if (inputAxis.y == 0)
        {
            desealerationSpeed = _moveSpeed;
        }
        if (_currentMoveSpeed > 0)
        {
            _currentMoveSpeed -= Time.fixedDeltaTime * desealerationSpeed;
        }
        else
        {
            _currentMoveSpeed += Time.fixedDeltaTime* desealerationSpeed;
        }
    }
}
