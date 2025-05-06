using UnityEngine;

public class DroneCharacter : CharacterBase
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _acceleration = 4f;
    [SerializeField] private float _gravity = -4f;
    [SerializeField] private float _flyingForce = 4f;
    [SerializeField] private float _visualLeanAmount = 25f;
    [SerializeField] private float _visualLeanSpeed = 5f;


    private float _verticalVelocity;
    private Vector3 _horizontalVelocity;
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
        Vector3 desiredHorizontalMovement = direction * _speed;

        if (_controller.isGrounded)
        {
            desiredHorizontalMovement = Vector3.zero;
            if (!_tryingToFly)
                _verticalVelocity = -0.1f;
        }
        else
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime;
        }

        if (_tryingToFly)
            _verticalVelocity += _flyingForce * Time.fixedDeltaTime;

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
