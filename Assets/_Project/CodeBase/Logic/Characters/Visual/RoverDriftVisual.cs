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


        private void Update()
        {
            if (RoverCharacter == null)
                return;

            if (RoverCharacter.IsDrifting)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    Quaternion.Euler(0, 0, -RoverCharacter.DriftAxis.x * 20), Time.deltaTime * 10);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
            }
            
            
            HandleTrails();


        }

        public void SetParticlesActivity(bool boostReady, bool boostActive)
        {
            if (boostReady)
            {
                if (!BoostReadyParticles.isPlaying) BoostReadyParticles.Play();
            }else
            {
                BoostReadyParticles.Stop();
            }

            if (boostActive)
            {
                if (!BoostParticles.isPlaying)BoostParticles.Play();
            }
            else
            {
                BoostParticles.Stop();
            }
        }

        private void HandleTrails()
        {
            foreach (var trailRenderer in LineRenderersContainer.GetComponentsInChildren<TrailRenderer>())
            {
                trailRenderer.emitting = RoverCharacter.IsDrifting;
            }
        }
    }
}