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

        

        public void SetParticlesActivity(bool boostReady, bool boostActive, bool grounded, bool drifting,
            float driftBoost)
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
            
            
            
            HandleTrails(grounded, drifting);
            if (boostReady)
            {
                Debug.Log(driftBoost);
                if (!BoostReadyParticles.isPlaying) BoostReadyParticles.Play();
                if (driftBoost > 10f)
                {
                    //set color
                    var main = BoostReadyParticles.main;
                    main.startColor = Color.magenta;
                }else if (driftBoost > 5f)
                {
                    //set color
                    var main = BoostReadyParticles.main;
                    main.startColor = Color.yellow;
                }
                else
                {
                    //set color
                    var main = BoostReadyParticles.main;
                    main.startColor = Color.blue;
                }
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

        private void HandleTrails(bool grounded, bool drifting)
        {
            
            foreach (var trailRenderer in LineRenderersContainer.GetComponentsInChildren<TrailRenderer>())
            {
                trailRenderer.emitting = drifting && grounded;
            }
        }
    }
}