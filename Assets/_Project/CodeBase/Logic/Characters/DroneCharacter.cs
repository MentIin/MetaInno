using CodeBase.Logic.Characters;
using UnityEngine;

public class DroneCharacter : CharacterBase
{
    [SerializeField] private LayerMask _bounceMask;
    
    
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

    
    
    
    private RaycastHit[] hits = new RaycastHit[4];
    
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

    public override void Move(Vector2 _inputAxis)
    {
        _reload -= Time.fixedDeltaTime;
        
        Vector3 direction = new Vector3(_inputAxis.x, 0f, _inputAxis.y);
        Vector3 desiredHorizontalMovement = direction * _speed;
        
        if (_inputAxis.sqrMagnitude != 0)
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
        

        Vector3 targetEuler = _visuals.eulerAngles;
        float amount = (_horizontalVelocity.magnitude / _speed) * _visualLeanAmount;
        targetEuler.z = -_horizontalVelocity.x * _speed;
        targetEuler.x = _horizontalVelocity.z * _speed;

        Quaternion targetRotation = Quaternion.Euler(targetEuler);
        //_visuals.rotation = Quaternion.Slerp(_visuals.rotation, targetRotation, _visualLeanSpeed * Time.fixedDeltaTime);


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
}
