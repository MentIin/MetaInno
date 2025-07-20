using System;
using CodeBase.Logic.Characters;
using CodeBase.Logic.Characters.Hands;
using UnityEditor.Animations;
using UnityEngine;

public class InnikCharacter : CharacterBase
{
    [SerializeField] private LayerMask _bounceMask;
    [SerializeField] private LayerMask _grabbableMask;

    [SerializeField] private Transform _hand1;
    [SerializeField] private Transform _hand2;

    private float gravity = -15f;
    private float _yVelocity = 0f;

    private float _jumpBuffer = 0.0f;
    private float _cayoutTime = 0.0f;
    private float _speed = 7.5f;
    private float _jumpHeight = 3f;
    private float _currentSpeed = 7.5f;
    private Vector3 _horizontalMovement = Vector3.zero;
    private float _hopSpeed = 14.5f;
    private int _hopsToMaxSpeed = 5;
    private int _hopCount = 0;
    private float _hopBuffer = 0;
    private bool _previouslyGrounded = false;

    private HandsController _handsController;
    [SerializeField] private Animator _animator;

    private RaycastHit[] hits = new RaycastHit[4];


    public override void Initialize()
    {
        _handsController = new HandsController(_hand1, _hand2, _grabbableMask, transform);
    }

    public override void OnCharacterEquipped()
    {
        base.OnCharacterEquipped();
        _horizontalMovement = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z) * Time.fixedDeltaTime;
        _yVelocity = _controller.velocity.y;
        _hopCount = 0;
    }

    public override void ActionStart()
    {
        _jumpBuffer = 0.3f;
        if (_controller.isGrounded)
            _cayoutTime = 0f;

    }

    public override void ActionStop()
    {
        if (_yVelocity > 0)
        {
            _yVelocity *= 0.5f;
        }
    }

    public override void SecondaryActionStart()
    {
        _handsController.SetActivePosition();
    }

    public override void SecondaryActionStop()
    {
        _handsController.DeactivateHands();
    }

    private void FixedUpdate()
    {
        if (IsOwner) _handsController.Tick();
        
        _animator.SetFloat("speed", _horizontalMovement.magnitude);
        _animator.SetBool("grounded", _controller.isGrounded);
    }


    public override void Move(Vector2 _inputAxis)
    {
        //_handsController.Tick();
        JumpUpdate();


        Vector3 direction = Vector3.forward * _inputAxis.y + Vector3.right * _inputAxis.x;
        float speedAdditionPerHop = (_hopSpeed - _speed) / _hopsToMaxSpeed;
        float speedAddition = Mathf.Clamp(_hopCount, 0, _hopsToMaxSpeed) * speedAdditionPerHop;
        _currentSpeed = _speed + speedAddition;
        Vector3 horizontalMovement = direction * (Time.fixedDeltaTime * _currentSpeed);
        Vector3 verticalMovement = Vector3.up * (_yVelocity * Time.fixedDeltaTime);
        Vector3 bounceMovement = _externalForceController.ExternalForce * Time.fixedDeltaTime;
        _horizontalMovement = Vector3.Lerp(_horizontalMovement, horizontalMovement, Time.fixedDeltaTime * 8f);
        _controller.Move(_horizontalMovement + verticalMovement);

        if (_inputAxis.sqrMagnitude != 0)
        {
            var desiredRotation = Quaternion.LookRotation(direction);
            _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, desiredRotation, Time.fixedDeltaTime * 10f);
        }



        if (_controller.isGrounded)
        {
            _yVelocity = -0.02f; // If reset to zero, will cause true/false flickering
            _cayoutTime = 0.3f;
        }
        else
        {
            _yVelocity += gravity * Time.fixedDeltaTime;
        }


        if (_inputAxis.sqrMagnitude != 0)
        {
            HandleBounce(horizontalMovement);
        }

        if (!_previouslyGrounded && _controller.isGrounded)
            _hopBuffer = 0.1f;
            
        _previouslyGrounded = _controller.isGrounded;
    }
    private void HandleBounce(Vector3 moveVector)
    {

        int c = Physics.SphereCastNonAlloc(_controller.center + _controller.transform.position, _controller.radius * 1f,
            moveVector.normalized, hits, .5f, _bounceMask);
        for (int i = 0; i < c; i++)
        {
            if (hits[i].collider == null) continue;
            if (hits[i].transform == _controller.transform) continue;


            float force = 2f;

            if (hits[i].transform.gameObject.CompareTag("Player"))
            {

                Debug.Log("BounceRPC player" + hits[i].transform.gameObject.GetComponent<ExternalForceController>().ExternalForce);
                if (hits[i].transform.gameObject.GetComponent<PlayerController>().CurrentCharacter is
                        InnikCharacter)
                {
                    force *= 2;
                }
                hits[i].transform.gameObject.GetComponent<ExternalForceController>().BounceRPC(-hits[i].normal, force);
            }
        }
    }


    private void JumpUpdate()
    {
        _jumpBuffer -= Time.fixedDeltaTime;
        _cayoutTime -= Time.fixedDeltaTime;
        
        if (_controller.isGrounded)
            _hopBuffer -= Time.fixedDeltaTime;
        if (_hopBuffer <= 0)
            _hopCount = 0;

        if (_jumpBuffer > 0)
        {
            if (_cayoutTime > 0)
            {
                _yVelocity = Mathf.Sqrt(2 * -gravity * _jumpHeight);
                _jumpBuffer = 0f;
                _cayoutTime = 0f;

                if (_hopBuffer > 0)
                    _hopCount++;
            }
        }
    }
}
