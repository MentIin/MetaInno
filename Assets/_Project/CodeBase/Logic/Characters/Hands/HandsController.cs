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
            _hand1TargetPosition = _innikTransform.position + new Vector3(0f, 0.2f, 0f) +
                                   _innikTransform.forward * 0.9f + _innikTransform.right * 0.5f;
            _hand2TargetPosition = _innikTransform.position + new Vector3(0f, 0.2f, 0f) +
                                   _innikTransform.forward * 0.9f + _innikTransform.right * -0.5f;
        }


        public void DeactivateHands()
        {
            _handsState = HandsState.Deactivated;
            foreach (var keyValuePair in _grabbables)
            {
                keyValuePair.Value.transform.SetParent(null);
            }
            _grabbables.Clear();
            
            // since there are only 1 character we can hardcode the hands positions
            
            _hand1TargetPosition = _innikTransform.position + new Vector3(0f, -0.3f, 0f) + _innikTransform.right * 0.5f;
            _hand2TargetPosition = _innikTransform.position + new Vector3(0f, -0.3f, 0f) + _innikTransform.right * -0.5f;
        }


        private void Check()
        {
            //CheckHand(_hand1);
            //CheckHand(_hand2);

            Vector3 pos = Vector3.Lerp(_hand1.position, _hand2.position, 0.5f);
            if (Physics.OverlapSphereNonAlloc(pos, 1f, _hits, _mask) != 0)
            {
                if (_hits[0].TryGetComponent<Grabbable>(out _currentGrabbable))
                {
                    Debug.Log("DETECTED GRABBABLE");
                    _handsState = HandsState.Grabbing;
                    _hand1TargetPosition = _currentGrabbable.GetGrabPoint(_hand1);
                    _hand2TargetPosition = _currentGrabbable.GetGrabPoint(_hand2);
                }else
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
                if ((_currentGrabbable.transform.position - _innikTransform.position).sqrMagnitude > _maxGrabDistance * _maxGrabDistance)
                {
                    SetActivePosition();
                }
            }
            

        }

        private void CheckHand(Transform hand)
        {
            if (Physics.OverlapSphereNonAlloc(hand.transform.position, 0.2f, _hits, _mask) != 0)
            {
                if (_hits[0].TryGetComponent<Grabbable>(out Grabbable grabbable))
                {
                    // TODO
                    
                    grabbable.Grab(hand);
                    if (!_grabbables.ContainsKey(hand))
                    {
                        _grabbables.Add(hand, grabbable);
                    }
                }
            }
        }

        public void Tick()
        {
            

            _hand1.position = Vector3.Lerp(_hand1.position, _hand1TargetPosition, 0.1f);
            _hand2.position = Vector3.Lerp(_hand2.position, _hand2TargetPosition, 0.1f);

            if (_handsState == HandsState.Deactivated)
            {
                DeactivateHands();
            }
            
            if (_handsState == HandsState.Activated)
            {
                SetActivePosition();
            }
            
            if (_handsState == HandsState.Activated || _handsState == HandsState.Grabbing)
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