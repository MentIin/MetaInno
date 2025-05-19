using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic.Characters.Hands
{
    public class HandsController
    {
        private Transform _hand1;
        private Transform _hand2;
        
        private Vector3 _hand1TargetPosition;
        private Vector3 _hand2TargetPosition;
        
        private Quaternion _hand1TargetRotation;
        private Quaternion _hand2TargetRotation;
        
        
        private LayerMask _mask;
        private Dictionary<Transform, Grabbable> _grabbables = new Dictionary<Transform, Grabbable>();
        private Collider[] _hits = new Collider[3];
        
        
        private float _maxGrabDistance = 2f;
        private HandsState _handsState = HandsState.Deactivated;
        
        private Grabbable _currentGrabbable;
        private readonly Transform _innikTransform;

        public HandsController(Transform hand1, Transform hand2, LayerMask mask, Transform innikTransform)
        {
            _innikTransform = innikTransform;
            _mask = mask;
            _hand1 = hand1;
            _hand2 = hand2;

            DeactivateHands();
        }



        public void SetActivePosition()
        {
            _handsState = HandsState.Activated;
            //_hand1.localPosition = new Vector3(0.5f, 0.2f, .9f);
            //_hand2.localPosition = new Vector3(-0.5f, 0.2f, .9f);
            
            
            /*_hand1TargetPosition = _innikTransform.position + new Vector3(0f, 0.2f, 0f) +
                                   _innikTransform.forward * 0.9f + _innikTransform.right * 0.5f;*/
            
            
            
            _hand1TargetPosition = new Vector3(-0.5f, 0.2f, 0.9f);
            
            
            _hand2TargetPosition = new Vector3(0.5f, 0.2f, 0.9f);
        }

        private Vector3 FromLocalToWorld(Vector3 vector3)
        {
            return _innikTransform.TransformPoint(vector3);
        }


        public void DeactivateHands()
        {
            if (_currentGrabbable != null)
            {
                _currentGrabbable.Ungrab();
                _currentGrabbable = null;
            }
            _handsState = HandsState.Deactivated;
            foreach (var keyValuePair in _grabbables)
            {
                keyValuePair.Value.transform.SetParent(null);
            }
            _grabbables.Clear();
            
            // since there are only 1 character we can hardcode the hands positions

            _hand1TargetPosition = new Vector3(-0.5f, -.7f, 0.5f);
            _hand2TargetPosition = new Vector3(0.5f, -.7f, 0.5f);
        }


        private void Check()
        {
            //CheckHand(_hand1);
            //CheckHand(_hand2);
            Vector3 pos = Vector3.Lerp(_hand1.position, _hand2.position, 0.5f);
            
            
            if (_handsState == HandsState.Grabbed || _handsState == HandsState.Grabbing || Physics.OverlapSphereNonAlloc(pos, 0.7f, _hits, _mask) != 0)
            {
                Collider collider = null;
                foreach (var VARIABLE in _hits)
                {
                    if (VARIABLE == null) continue;
                    
                    
                    if (VARIABLE.transform != _innikTransform.parent.parent)
                    {
                        Debug.Log(VARIABLE.transform.gameObject.name + "    " + _innikTransform.parent.parent.gameObject.name);
                        collider = VARIABLE;
                    }
                }

                bool isGrabbable = false;
                if (collider)
                {
                    isGrabbable = collider.TryGetComponent<Grabbable>(out _currentGrabbable);
                }
                
                if (_handsState == HandsState.Grabbed || _handsState == HandsState.Grabbing || isGrabbable)
                {
                    
                    if (_handsState != HandsState.Grabbing && _handsState != HandsState.Grabbed)
                    {
                        UpdateGrabbablePoints();
                        _handsState = HandsState.Grabbing;
                    }

                    if (_handsState == HandsState.Grabbing)
                    {
                        UpdateGrabbablePoints();
                    }
                    
                    if ((_innikTransform.InverseTransformPoint(_hand1.position) - _hand1TargetPosition).sqrMagnitude < 0.05f)
                    {
                        
                        if (_handsState == HandsState.Grabbing)
                        {
                            UpdateGrabbablePoints();
                            _handsState = HandsState.Grabbed;
                            _currentGrabbable.Grab(_hand1);
                        }
                        
                        _handsState = HandsState.Grabbed;
                        
                        Vector3 targetMidpoint = Vector3.Lerp(_hand1TargetPosition, _hand2TargetPosition, 0.5f);

                        if (_innikTransform.position.y +0.2f > _currentGrabbable.transform.position.y)
                        {
                            _hand1TargetPosition += Vector3.up * Time.fixedDeltaTime * 3;
                            _hand2TargetPosition += Vector3.up * Time.fixedDeltaTime * 3;
                        }
                        
                        // z
                        float zDist = (_innikTransform.position - _currentGrabbable.transform.position).sqrMagnitude;
                        if (zDist < 1.2f)
                        {
                            _hand1TargetPosition.z += Time.fixedDeltaTime * 3;
                            _hand2TargetPosition.z += Time.fixedDeltaTime * 3;
                        }
                        else if (zDist > 1.3f) 
                        {
                            _hand1TargetPosition.z -= Time.fixedDeltaTime * 3;
                            _hand2TargetPosition.z -= Time.fixedDeltaTime * 3;
                        }
                        
                        HandleX(targetMidpoint);
                        //HandleZ(targetMidpoint);
                    }
                }
                else
                {
                    _handsState = HandsState.Activated;
                }
            }
            else
            {
                _handsState = HandsState.Activated;
            }

            if (_currentGrabbable != null)
            {
                if ((_currentGrabbable.transform.position - _hand1.transform.position).sqrMagnitude > _maxGrabDistance * _maxGrabDistance)
                {
                    SetActivePosition();
                }
                if (_handsState != HandsState.Grabbing && _handsState != HandsState.Grabbed)
                {
                    _handsState = HandsState.Activated;
                    _currentGrabbable.Ungrab();
                }
            }
        }

        private void UpdateGrabbablePoints()
        {
            _hand1TargetPosition = _currentGrabbable.GetGrabPoint(_hand1);
            _hand1TargetPosition = _innikTransform.InverseTransformPoint(_hand1TargetPosition);
            _hand2TargetPosition = _currentGrabbable.GetGrabPoint(_hand2);
            _hand2TargetPosition = _innikTransform.InverseTransformPoint(_hand2TargetPosition);
        }

        private void HandleZ(Vector3 targetMidpoint)
        {
            float currentDistance = (targetMidpoint - _innikTransform.position).magnitude; // Текущая дистанция цели от игрока
            float desiredDistance = 1.1f; // *** Ваше желаемое расстояние удержания! Убедитесь, что оно совпадает с тем, что вы используете для расчета базовой цели в состоянии Grabbing! ***
            float forwardBackwardSpeed = 5f; // Скорость приближения/отдаления целей (можете настроить)
            float distanceThreshold = 0.05f; // Порог для дистанции, чтобы избежать дрожания у цели (можете настроить)

            if (currentDistance > desiredDistance + distanceThreshold)
            {
                Vector3 moveVector = _innikTransform.forward * forwardBackwardSpeed * Time.fixedDeltaTime; // Используем Time.fixedDeltaTime
                _hand1TargetPosition -= moveVector;
                _hand2TargetPosition -= moveVector;
            }
            else if (currentDistance < desiredDistance - distanceThreshold)
            {
                Vector3 moveVector = _innikTransform.forward * forwardBackwardSpeed * Time.fixedDeltaTime;
                _hand1TargetPosition += moveVector;
                _hand2TargetPosition += moveVector;
            }
        }

        private void HandleX(Vector3 targetMidpoint)
        {
            if (targetMidpoint.x > .2f)
            {
                _hand1TargetPosition.x -= Time.fixedDeltaTime * 3;
                _hand2TargetPosition.x -= Time.fixedDeltaTime * 3;
            }
            else if (targetMidpoint.x < -.2f)
            {
                _hand1TargetPosition.x += Time.fixedDeltaTime * 3;
                _hand2TargetPosition.x += Time.fixedDeltaTime * 3;
            }
        }

        private void CheckHand(Transform hand)
        {
            if (Physics.OverlapSphereNonAlloc(hand.transform.position, 0.2f, _hits, _mask) != 0)
            {
                if (_hits[0].TryGetComponent<Grabbable>(out Grabbable grabbable))
                {
                    // TODO
                    
                    //grabbable.GrabServerRpc(hand);
                    if (!_grabbables.ContainsKey(hand))
                    {
                        _grabbables.Add(hand, grabbable);
                    }
                }
            }
        }

        public void Tick()
        {
            

            _hand1.position = Vector3.Lerp(_hand1.position, FromLocalToWorld(_hand1TargetPosition), 0.4f);
            _hand2.position = Vector3.Lerp(_hand2.position, FromLocalToWorld(_hand2TargetPosition), 0.4f);

            _hand1.rotation = _innikTransform.rotation;
            _hand2.rotation = _innikTransform.rotation;

            if (_handsState == HandsState.Deactivated)
            {
                DeactivateHands();
            }
            
            if (_handsState == HandsState.Activated)
            {
                SetActivePosition();
            }
            
            if (_handsState == HandsState.Activated || _handsState == HandsState.Grabbing || _handsState == HandsState.Grabbed)
            {
                Check();
            }
        }
    }

    internal enum HandsState
    {
        None=0,Deactivated=1, Activated=2, Grabbing=3, Grabbed=4
    }
}