using UnityEngine;
using UnityEngine.UIElements;

public class RoverCharacter : CharacterBase
{
    private float gravity=-12f;
    private float _yVelocity=0f;

    private bool _drift;
    
    private Vector3 _driftVelocity;
    
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
        _yVelocity += gravity * Time.fixedDeltaTime;
        if (_controller.isGrounded) _yVelocity = 0;
        
        
        Vector3 targetRotation = new Vector3(inputAxis.x, 0, inputAxis.y);

        Vector3 currentRotation = _controller.transform.forward;


        Vector3 direction = new Vector3();
        if (inputAxis.sqrMagnitude != 0)
        {

            if (Mathf.Abs(Vector3.Angle(targetRotation, currentRotation)) < 5f)
            {
                direction += currentRotation;
            }
            else if (Mathf.Abs(Vector3.Angle(-targetRotation, currentRotation)) < 5f)
            {
                direction -= currentRotation;
            }
            else
            {
                float r = Vector3.SignedAngle(currentRotation, targetRotation, Vector3.up);
                if (Mathf.Abs(r) > 90)
                {
                    r *= -1;
                    
                    
                    if (_drift)
                    {
                        _driftVelocity -= currentRotation*Time.fixedDeltaTime;
                    }
                    
                }
                else
                {
                    if (_drift)
                    {
                        _driftVelocity += currentRotation*Time.fixedDeltaTime;
                    }
                }
                r = Mathf.Clamp(r * 1000, -Time.fixedDeltaTime, Time.fixedDeltaTime);
                _controller.transform.Rotate(0f, r * 60, 0f);
            }
        }



        _driftVelocity *= 0.9f;
        

        direction += Vector3.up * _yVelocity;
        _controller.Move(direction * Time.fixedDeltaTime * 4 + _driftVelocity);
    }
}
