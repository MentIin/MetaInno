using UnityEngine;

public class DroneCharacter : CharacterBase
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _acceleration = 4f;
    [SerializeField] private float _gravity = -6f;
    [SerializeField] private float _flyingForce = 4f;
    [SerializeField] private float _visualLeanAmount = 25f;
    [SerializeField] private float _visualLeanSpeed = 5f;


    private float _verticalVelocity;
    private Vector3 _horizontalVelocity;
    private bool _tryingToFly = false;
    
    private float _reload=0f;

    public override void ActionStart()
    {
        if (_reload > 0) return;
        
        _reload = 0.2f;
        _verticalVelocity += 7f;
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
            _controller.transform.rotation = Quaternion.LookRotation(direction);
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
        _visuals.rotation = Quaternion.Slerp(_visuals.rotation, targetRotation, _visualLeanSpeed * Time.fixedDeltaTime);
    }
}
