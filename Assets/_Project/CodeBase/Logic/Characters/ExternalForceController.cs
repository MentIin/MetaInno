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
        
        public void Bounce(Vector3 hitInfoNormal, float speed)
        {
            
            Vector3 externalForce = hitInfoNormal * speed + Vector3.up * speed;
            _externalForces.Add(new ExternalForce(externalForce, 0.5f));;
        }


        private void FixedUpdate()
        {
            ExternalForce = Vector3.zero;
            foreach (var force in _externalForces)
            {
                ExternalForce += force.Force;
                force.Tick(Time.fixedDeltaTime);
            }
            
            //TODO optimize
            _externalForces.RemoveAll(x => x.IsFinished);
        }
    }
    
    internal class ExternalForce
    {
        public Vector3 Force;
        public float Duration;
        
        public bool IsFinished => _timeLeft <= 0;
        private float _timeLeft;
        
        public ExternalForce(Vector3 force, float duration)
        {
            Force = force;
            Duration = duration;
            _timeLeft = duration;
        }

        public void Tick(float delta)
        {
            Force *= 0.96f;
            _timeLeft -= delta;
        }
    }
}