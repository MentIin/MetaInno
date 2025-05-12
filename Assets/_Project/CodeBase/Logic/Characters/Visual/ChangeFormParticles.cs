using System;
using UnityEngine;

namespace CodeBase.Logic.Characters.Visual
{
    public class ChangeFormParticles : MonoBehaviour
    {
        public PlayerController PlayerController;
        public ParticleSystem Particles;


        private void OnEnable()
        {
            PlayerController.CurrentCharacterChanged += CurrentCharacterChanged;
        }

        private void OnDisable()
        {
            PlayerController.CurrentCharacterChanged -= CurrentCharacterChanged;
        }

        private void CurrentCharacterChanged()
        {
            Particles.Play();
        }
    }
}