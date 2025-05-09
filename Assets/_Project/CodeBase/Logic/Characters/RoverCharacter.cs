using CodeBase.Logic.Characters.Visual;
using FishNet.Object;
using UnityEngine;

public class RoverCharacter : CharacterBase
{
    [SerializeField] private RoverDriftVisual _roverDriftVisual;
    
    [Tooltip("Which layer the rover can bounce off (walls, etc)")]
    [SerializeField] private LayerMask _bounceMask;
    
    private float gravity=-12f;
    private float _yVelocity=0f;

    private float _currentMoveSpeed = 0f;
    private float _moveSpeed = 11f;

    private bool _tryingDrift;
    private bool _drift;
    
    private Vector3 _driftVelocity;
    private float _rotationSpeed = 60;
    
    private Vector2 driftAxis;
    
    private float _readyDriftBoost=0f;
    private float _driftBoost=0f;
    private float _minimumBoost = 3f;
    public bool IsDrifting => _drift;
    public Vector2 DriftAxis => driftAxis;
    
    public bool BoostReady => _readyDriftBoost > _minimumBoost;
    public bool BoostActive => _driftBoost > 0;


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
        if (!Mathf.Approximately(Mathf.Sign(inputAxis.x), Mathf.Sign(driftAxis.x)) || inputAxis.y <= 0)
        {
            if (_drift)
            {
                inputAxis = driftAxis;
            }
        }else
        {
            driftAxis = inputAxis;
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

        if (_tryingDrift && !_drift && inputAxis.x != 0 && inputAxis.y > 0)
        {
            _drift = true;
            driftAxis = inputAxis; 
        }
        else if ((!_tryingDrift && _drift))
        {
            _drift = false;
            if (_readyDriftBoost < _minimumBoost)
            {
                _readyDriftBoost = 0f;
            }

            _driftBoost += _readyDriftBoost;
        }

        if (_drift)
        {
            _readyDriftBoost += Time.fixedDeltaTime*2;
            _driftVelocity = -_controller.transform.right * Mathf.Sign(inputAxis.x) * 2;
        }
        else
        {
            _readyDriftBoost = 0f;
            _driftVelocity = _controller.transform.forward * _readyDriftBoost;
            _driftBoost -= Time.fixedDeltaTime * 3;
            if (_driftBoost < 0)
            {
                _driftBoost = 0;
            }
        }
        
        

        Vector3 currentRotationVector = _controller.transform.forward;
        
        _currentMoveSpeed = Mathf.Clamp((_currentMoveSpeed + inputAxis.y*Time.fixedDeltaTime * 7) ,-_moveSpeed, _moveSpeed);
        
        Vector3 direction = currentRotationVector.normalized * Time.fixedDeltaTime * (_currentMoveSpeed + _driftBoost);
        direction += Vector3.up * _yVelocity * Time.fixedDeltaTime;
        
        
        float rotationFactor = inputAxis.x;
        if (_drift) rotationFactor += 0.4f * Mathf.Sign(inputAxis.x);
        _controller.transform.Rotate(0f,  rotationFactor* _rotationSpeed * Time.fixedDeltaTime, 0f);

        float mod = 1f;
        if (_drift)
        {
            mod = 0.7f;
        }
        
        
        _driftVelocity *= 0.9f;
        
        
        
        SendDriftDataToClient(BoostReady, BoostActive, _controller.isGrounded);

        

        Vector3 finalMoveVector = direction * mod + _driftVelocity * Time.fixedDeltaTime +
                             _externalForceController.ExternalForce * Time.fixedDeltaTime;

        HandleBounce(finalMoveVector);
        _controller.Move(finalMoveVector);
    }

    private void HandleBounce(Vector3 moveVector)
    {
        if (!_drift && _driftBoost < 1f) return;
        
        if (_controller.collisionFlags == CollisionFlags.Sides)
        {
            RaycastHit hit;
            if (Physics.SphereCast(_controller.center, _controller.radius, moveVector, out hit) )
            {

                if (_drift)
                {
                    _driftBoost = 0f;
                    _readyDriftBoost = 0f;
                    _currentMoveSpeed = 0f;
                    _externalForceController.Bounce(hit.normal * 2f, _minimumBoost);
                }
                else
                {
                    _currentMoveSpeed = 0f;
                    _driftBoost = 0f;
                    _readyDriftBoost = 0f;
                    _externalForceController.Bounce(hit.normal * 2f, _driftBoost+_moveSpeed);
                }
            }

        }
    }

    [ObserversRpc]
    private void SendDriftDataToClient(bool ready, bool boost, bool grounded)
    {
       _roverDriftVisual.SetParticlesActivity(ready, boost, grounded);
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
