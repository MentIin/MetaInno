using CodeBase.Logic.Characters;
using UnityEngine;

public class DroneCharacter : CharacterBase
{
    [SerializeField] private LayerMask _bounceMask;
    [SerializeField] private LayerMask _groundMask;

    private float _speed = 10f;
    private float _acceleration = 8f;
    private float _gravity = -24f;
    private float _flyingForce = 12f;
    private float _maxHeight = 12f;

    [SerializeField] private float _visualLeanAmount = -14f;
    [SerializeField] private float _visualLeanSpeed = 6f;


    private float _verticalVelocity;
    private Vector3 _horizontalVelocity;
    private bool _tryingToFly = false;

    private float _reload = 0f;



    private RaycastHit[] hits = new RaycastHit[4];

    public override void OnCharacterEquipped()
    {
        base.OnCharacterEquipped();
        _horizontalVelocity = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z);
        
        if (_controller.isGrounded)
            _verticalVelocity = 10f;
        else
            _verticalVelocity = _controller.velocity.y;
    }

    public override void ActionStart()
    {
        if (_reload > 0 || !HasGround()) return;

        _reload = 0.3f;
        if (_verticalVelocity < 0)
            _verticalVelocity = 0;

        _verticalVelocity += 12f;
    }

    public override void ActionStop()
    {
        // nothing
    }

    public override void Move(Vector2 _inputAxis)
    {
        Debug.Log(HasGround());
        _reload -= Time.fixedDeltaTime;

        Vector3 direction = new Vector3(_inputAxis.x, 0f, _inputAxis.y);
        Vector3 desiredHorizontalMovement = direction * _speed;

        if (_inputAxis.sqrMagnitude != 0)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            _controller.transform.localRotation = Quaternion.Slerp(_controller.transform.localRotation, desiredRotation, Time.deltaTime * 7f);
        }


        if (_controller.isGrounded)
        {
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
        

        Vector3 burger = new Vector3(
            -direction.z,  // Pitch (forward/backward)
            0, // Yaw (we typically don't want this for leaning)
            direction.x  // Roll (left/right)
        );
        Vector3 targetEuler = _visuals.transform.InverseTransformDirection(burger);
        targetEuler *= _visualLeanAmount;



        Quaternion targetRotation = Quaternion.Euler(targetEuler);
        if (_controller.isGrounded)
            _visuals.localRotation = Quaternion.Slerp(_visuals.localRotation, Quaternion.identity, _visualLeanSpeed * 2 * Time.fixedDeltaTime);
        else
            _visuals.localRotation = Quaternion.Slerp(_visuals.localRotation, targetRotation, _visualLeanSpeed * Time.fixedDeltaTime);


        Vector3 moveVector = (_horizontalVelocity + upVelocity) * Time.fixedDeltaTime +
                             _externalForceController.ExternalForce * Time.fixedDeltaTime;
        _controller.Move(moveVector);

        HandleBounce(moveVector);
    }

    private void HandleBounce(Vector3 moveVector)
    {
        if (moveVector.sqrMagnitude == 0) return;

        if (_controller.detectCollisions)
        {

            int c = Physics.SphereCastNonAlloc(_controller.center + _controller.transform.position, _controller.radius * 0.8f,
                moveVector.normalized, hits, .5f, _bounceMask);
            for (int i = 0; i < c; i++)
            {
                if (hits[i].collider == null) continue;
                if (hits[i].transform == _controller.transform) continue;


                float force = _horizontalVelocity.magnitude;

                if (hits[i].transform.gameObject.CompareTag("Player"))
                {
                    Debug.Log("BounceRPC player" + hits[i].transform.gameObject.GetComponent<ExternalForceController>().ExternalForce);
                    if (hits[i].transform.gameObject.GetComponent<PlayerController>().CurrentCharacter is
                            InnikCharacter ||
                        hits[i].transform.gameObject.GetComponent<PlayerController>().CurrentCharacter is
                            RoverCharacter)
                    {

                        return;
                    }
                    hits[i].transform.gameObject.GetComponent<ExternalForceController>().BounceRPC(-hits[i].normal, force);
                }
                else
                {
                    _externalForceController.BounceRPC(hits[i].normal * 2f, _horizontalVelocity.magnitude);
                    _horizontalVelocity = new Vector3();
                }
            }
        }
    }

    public override void SecondaryActionStart()
    {
        Debug.LogWarning("not implemented");
    }
    public override void SecondaryActionStop()
    {
        Debug.LogWarning("not implemented");
    }

    private bool HasGround()
    {
        return Physics.Raycast(_controller.transform.position, Vector3.down, out RaycastHit hit, _maxHeight, _groundMask);
    }
}
