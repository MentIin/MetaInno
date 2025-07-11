using CodeBase.Logic.Characters;
using CodeBase.Logic.Characters.Visual;
using DG.Tweening.Core;
using FishNet.Object;
using UnityEngine;
using Debugger = System.Diagnostics.Debugger;

public class RoverCharacter : CharacterBase
{
    [SerializeField] private RoverDriftVisual _roverDriftVisual;

    [Tooltip("Which layer the rover can bounce off (walls, etc)")]
    [SerializeField] private LayerMask _bounceMask;

    private float gravity = -12f;
    private float _yVelocity = 0f;

    private float _currentMoveSpeed = 0f;
    private float _moveSpeed = 14f;

    private bool _tryingDrift;
    private bool _drift;

    private Vector3 _driftVelocity;
    private float _rotationSpeed = 60;

    private Vector2 driftAxis;

    private float _readyDriftBoost = 0f;
    private float _driftBoost = 0f;
    private float _minimumBoost = 3f;
    public bool IsDrifting => _drift;
    public Vector2 DriftAxis => driftAxis;

    public bool BoostReady => _readyDriftBoost > _minimumBoost;
    public bool BoostActive => _driftBoost > 0;


    private RaycastHit[] hits = new RaycastHit[4];


    public override void ActionStart()
    {
        _tryingDrift = true;
    }

    public override void ActionStop()
    {
        _tryingDrift = false;
    }

    public override void SecondaryActionStart()
    {
        Debug.LogWarning("not implemented");
    }
    public override void SecondaryActionStop()
    {
        Debug.LogWarning("not implemented");
    }

    public override void Move(Vector2 inputAxis)
    {
        if (!Mathf.Approximately(Mathf.Sign(inputAxis.x), Mathf.Sign(driftAxis.x)) || inputAxis.y <= 0)
        {
            if (_drift)
            {
                inputAxis = driftAxis;
            }
        }
        else
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
            StartDrift(inputAxis);
        }
        else if ((!_tryingDrift && _drift))
        {
            _drift = false;
            if (_readyDriftBoost < _minimumBoost)
            {
                _readyDriftBoost = 0f;
            }

            _driftBoost += _readyDriftBoost;
            
            // rotate to the direction of camera
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0; // Ignore vertical component
            cameraForward.Normalize();
            _controller.transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            
        }

        if (_drift)
        {
            _readyDriftBoost += Time.fixedDeltaTime * 2;
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

        _currentMoveSpeed = Mathf.Clamp((_currentMoveSpeed + inputAxis.y * Time.fixedDeltaTime * 7), -_moveSpeed, _moveSpeed);

        Vector3 direction = currentRotationVector.normalized * Time.fixedDeltaTime * (_currentMoveSpeed + _driftBoost);
        direction += Vector3.up * _yVelocity * Time.fixedDeltaTime;


        
        float rotationFactor = inputAxis.x;
        if (_drift) rotationFactor += 0.4f * Mathf.Sign(inputAxis.x);
        _controller.transform.Rotate(0f, rotationFactor * _rotationSpeed * Time.fixedDeltaTime, 0f);

        
        
        float mod = 1f;
        if (_drift)
        {
            mod = 0.7f;
        }


        _driftVelocity *= 0.9f;



        SendDriftDataToServer(BoostReady, BoostActive, _controller.isGrounded, _drift, _readyDriftBoost);
        _roverDriftVisual.SetParticlesActivity(BoostReady, BoostActive, _controller.isGrounded, _drift, _readyDriftBoost);



        Vector3 finalMoveVector = direction * mod + _driftVelocity * Time.fixedDeltaTime +
                             _externalForceController.ExternalForce * Time.fixedDeltaTime;

        HandleBounce(finalMoveVector);
        if (finalMoveVector.magnitude > Time.fixedDeltaTime / 2f) _controller.Move(finalMoveVector);
    }

    private void StartDrift(Vector2 inputAxis)
    {
        _externalForceController.BounceLocal(Vector3.up, 2f);
        _drift = true;
        driftAxis = inputAxis;
    }

    private void HandleBounce(Vector3 moveVector)
    {
        if (!_drift && _driftBoost + _currentMoveSpeed < _moveSpeed)
            return;


        int c = Physics.SphereCastNonAlloc(_controller.center + _controller.transform.position, _controller.radius * 0.8f,
            moveVector.normalized, hits, .8f, _bounceMask);
        for (int i = 0; i < c; i++)
        {
            if (hits[i].collider == null) continue;
            if (hits[i].transform == _controller.transform) continue;


            float force = Mathf.Max(_minimumBoost / 2f, _driftBoost + _currentMoveSpeed);

            if (hits[i].transform.gameObject.CompareTag("Player"))
            {
                hits[i].transform.gameObject.GetComponent<ExternalForceController>().BounceRPC(-hits[i].normal, force);
                Debug.Log("BounceRPC player" + hits[i].transform.gameObject.GetComponent<ExternalForceController>().ExternalForce);
                if (hits[i].transform.gameObject.GetComponent<PlayerController>().CurrentCharacter is
                        InnikCharacter ||
                    hits[i].transform.gameObject.GetComponent<PlayerController>().CurrentCharacter is
                        DroneCharacter)
                {

                    return;
                }
            }

            _currentMoveSpeed = 0f;
            _driftBoost = 0f;
            _readyDriftBoost = 0f;
            _externalForceController.BounceLocal(hits[i].normal * 2f, _driftBoost + _moveSpeed);



        }
    }

    [ObserversRpc]
    private void SendDriftDataToClient(bool ready, bool boost, bool grounded, bool drift, float driftBoost)
    {
        _roverDriftVisual.SetParticlesActivity(ready, boost, grounded, drift, driftBoost);
    }

    [ServerRpc]
    private void SendDriftDataToServer(bool ready, bool boost, bool grounded, bool drift, float driftBoost)
    {
        SendDriftDataToClient(ready, boost, grounded, drift, driftBoost);
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
            _currentMoveSpeed += Time.fixedDeltaTime * desealerationSpeed;
        }
    }


}
