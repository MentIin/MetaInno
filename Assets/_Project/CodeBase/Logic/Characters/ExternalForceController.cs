using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Characters
{
    public class ExternalForceController : NetworkBehaviour
    {
        public Vector3 ExternalForce { get; private set; }

        
        private List<ExternalForce> _externalForces = new List<ExternalForce>();


        private bool _zero = false;
        
        [ServerRpc(RequireOwnership = false)]
        public void BounceRPC(Vector3 hitInfoNormal, float speed)
        {
            //BounceLocal(hitInfoNormal, speed);
            BounceClients(hitInfoNormal, speed);
        }
        
        [ObserversRpc]
        private void BounceClients(Vector3 hitInfoNormal, float speed)
        {
            if (IsOwner) BounceLocal(hitInfoNormal, speed);
        }
        
        
        public void BounceLocal(Vector3 hitInfoNormal, float speed)
        {
            if (ExternalForce.magnitude > 0)
            {
                return;
            }
            Vector3 externalForce = hitInfoNormal * speed + Vector3.up * speed * 0.5f;
            _externalForces.Add(new ExternalForce(externalForce, 0.5f));;
        }

        
        private void FixedUpdate()
        {
            if (!base.IsOwner) return;
            
            
            ExternalForce = Vector3.zero;
            foreach (var force in _externalForces)
            {
                ExternalForce += force.Force;
                force.Tick(Time.fixedDeltaTime);
            }
            
            //TODO optimize
            _externalForces.RemoveAll(x => x.IsFinished);

            if (ExternalForce.sqrMagnitude == 0 && !_zero)
            {
                _zero = true;
                SetTotalForceClient(ExternalForce);
            }
            else
            {
                if (ExternalForce.sqrMagnitude == 0)
                {
                    // nothing
                }
                else
                {
                    _zero = false;
                    //SetTotalForceServer(ExternalForce);
                }
                
            }
            
        }
        [ServerRpc(RequireOwnership = false)]
        private void SetTotalForceServer(Vector3 force)
        {
            ExternalForce = force;
            SetTotalForceClient(force);
        }
        
        
        [ObserversRpc]
        private void SetTotalForceClient(Vector3 force)
        {
            ExternalForce = force;
        }
        
    }
    
    internal class ExternalForce
    {
        public Vector3 Force => _force * (_timeLeft / Duration);
        public float Duration;

        public Vector3 _force;
        public bool IsFinished => _timeLeft <= 0;
        private float _timeLeft;
        
        public ExternalForce(Vector3 force, float duration)
        {
            _force = force;
            Duration = duration;
            _timeLeft = duration;
        }

        public void Tick(float delta)
        {
            _timeLeft -= delta;
        }
    }
}