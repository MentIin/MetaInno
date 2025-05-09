using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Visual
{
    public class RoverDriftVisual : MonoBehaviour
    {
        public RoverCharacter RoverCharacter;
        public GameObject LineRenderersContainer;

        public ParticleSystem BoostReadyParticles;
        public ParticleSystem BoostParticles;

        

        public void SetParticlesActivity(bool boostReady, bool boostActive, bool grounded, bool drifting)
        {
            if (drifting)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    Quaternion.Euler(0, 0, -RoverCharacter.DriftAxis.x * 20), Time.deltaTime * 10);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
            }
            
            
            
            HandleTrails(grounded);
            if (boostReady)
            {
                if (!BoostReadyParticles.isPlaying) BoostReadyParticles.Play();
            }else
            {
                BoostReadyParticles.Stop();
            }

            if (boostActive && grounded)
            {
                if (!BoostParticles.isPlaying)BoostParticles.Play();
            }
            else
            {
                BoostParticles.Stop();
            }
        }

        private void HandleTrails(bool grounded)
        {
            
            foreach (var trailRenderer in LineRenderersContainer.GetComponentsInChildren<TrailRenderer>())
            {
                trailRenderer.emitting = RoverCharacter.IsDrifting && grounded;
            }
        }
    }
}