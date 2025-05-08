using UnityEngine;
using UnityEngine.UIElements;

public class RoverCharacter : CharacterBase
{
    private float gravity=-12f;
    private float _yVelocity=0f;

    private float _currentMoveSpeed = 0f;
    private float _moveSpeed = 7f;

    private bool _drift;
    
    private Vector3 _driftVelocity;
    private float _rotationSpeed = 60;
    

    public override void ActionStart()
    {
        _drift = true;
    }

    public override void ActionStop()
    {
        _drift = false;
    }

    public override void Move(Vector2 inputAxis)
    {
        if (inputAxis.y < 0)
        {
            if (Mathf.Abs(inputAxis.x) < 0.1f)
            {
                inputAxis.x = 0;
            }
        }


        float desealerationSpeed = 2f;
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

        
        _yVelocity += gravity * Time.fixedDeltaTime;
        if (_controller.isGrounded) _yVelocity = 0;
        
        Vector3 targetRotation = new Vector3(inputAxis.x, 0, inputAxis.y);
        Vector3 currentRotation = _controller.transform.forward;

        float mod = 1;
        if (Vector3.Angle(targetRotation, currentRotation) > 90f)
        {
            mod = -1f;
        }

        
        _currentMoveSpeed = Mathf.Clamp((_currentMoveSpeed + mod*Time.fixedDeltaTime * 5) ,-_moveSpeed, _moveSpeed);
        
        Vector3 direction = new Vector3();
        if (inputAxis.sqrMagnitude != 0)
        {
            direction = CalculateDirectionVector(targetRotation, currentRotation);
        }
        
        _driftVelocity *= 0.9f;

        direction = currentRotation.normalized * Time.fixedDeltaTime * _currentMoveSpeed;
        direction += Vector3.up * _yVelocity * Time.fixedDeltaTime;
        
        _controller.Move(direction + _driftVelocity);
    }

    private Vector3 CalculateDirectionVector(Vector3 targetRotation, Vector3 currentRotation)
    {
        Vector3 direction;
        
        if (Mathf.Abs(Vector3.Angle(targetRotation, currentRotation)) < 90f)
        {
            direction = currentRotation;
        }
        else 
        {
            direction = currentRotation;
        }

        float r = Vector3.SignedAngle(currentRotation, targetRotation, Vector3.up);

        if (Mathf.Abs(r) > 2f && Mathf.Abs(r) < 178f)
        {
            r = Mathf.Clamp(r * 1000, -Time.fixedDeltaTime, Time.fixedDeltaTime);
            _controller.transform.Rotate(0f, r * _rotationSpeed, 0f);
        }
        
        
        return direction;
    }
}
