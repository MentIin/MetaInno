using UnityEngine;

public class DroneCharacter : CharacterBase
{
    private float _speed = 5f;
    private float _acceleration = 10f;
    private float _gravity = -24f;
    private float _flyingForce = 12f;
    
    private float _visualLeanAmount = 2f;
    [SerializeField] private float _visualLeanSpeed = 12f;


    private float _verticalVelocity;
    private Vector3 _horizontalVelocity;
    private bool _tryingToFly = false;
    
    private float _reload=0f;

    public override void ActionStart()
    {
        if (_reload > 0) return;
        
        _reload = 0.3f;
        if (_verticalVelocity < 0) 
            _verticalVelocity = 0;
        
        _verticalVelocity += 12f;
    }

    public override void ActionStop()
    {
        // nothing
    }

    public override void Move(Vector2 inputAxis)
    {
        _reload -= Time.fixedDeltaTime;
        
        Vector3 direction = new Vector3(inputAxis.x, 0f, inputAxis.y);
        Vector3 desiredHorizontalMovement = direction * _speed;
        
        if (inputAxis.sqrMagnitude != 0)
        {
            _controller.transform.localRotation = Quaternion.LookRotation(direction);
        }
        
        
        
        if (_controller.isGrounded)
        {
            _verticalVelocity = Mathf.Clamp(_verticalVelocity, 0f, _verticalVelocity);
            desiredHorizontalMovement = Vector3.zero;
        }
        else
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime;
        }

        //if (_tryingToFly)
        //    _verticalVelocity += _flyingForce * Time.fixedDeltaTime;

        Vector3 upVelocity = Vector3.up * _verticalVelocity;
        
        _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, desiredHorizontalMovement, _acceleration * Time.fixedDeltaTime);
        
        _controller.Move((_horizontalVelocity + upVelocity) * Time.fixedDeltaTime);

        Vector3 targetEuler = _visuals.eulerAngles;
        float amount = (_horizontalVelocity.magnitude / _speed) * _visualLeanAmount;
        targetEuler.z = -_horizontalVelocity.x * _speed;
        targetEuler.x = _horizontalVelocity.z * _speed;

        Quaternion targetRotation = Quaternion.Euler(targetEuler);
        //_visuals.rotation = Quaternion.Slerp(_visuals.rotation, targetRotation, _visualLeanSpeed * Time.fixedDeltaTime);
    }
}
