using System;
using CodeBase.Logic.Characters;
using CodeBase.Logic.Characters.Hands;
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

    private HandsController _handsController;

    private RaycastHit[] hits = new RaycastHit[4];


    public override void Initialize()
    {
        _handsController = new HandsController(_hand1, _hand2, _grabbableMask, transform);

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
    }


    public override void Move(Vector2 _inputAxis)
    {
        //_handsController.Tick();
        JumpUpdate();


        Vector3 direction = Vector3.forward * _inputAxis.y + Vector3.right * _inputAxis.x;

        Vector3 moveVector = direction * Time.fixedDeltaTime * _speed +
                             Vector3.up * _yVelocity * Time.fixedDeltaTime +
                             _externalForceController.ExternalForce * Time.fixedDeltaTime;
        _controller.Move(moveVector);

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


        if (_inputAxis.sqrMagnitude != 0)
        {
            HandleBounce(moveVector);
        }

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
        if (_jumpBuffer > 0)
        {
            if (_cayoutTime > 0)
            {
                _yVelocity = 11f;
                _jumpBuffer = 0f;
                _cayoutTime = 0f;
            }
        }
    }
}
