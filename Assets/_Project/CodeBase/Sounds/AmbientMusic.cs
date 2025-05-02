using CodeBase.Infrastructure.Services.AudioService;
using UnityEngine;
//using UnityEngine.AddressableAssets;

namespace _Project.CodeBase.Sounds
{
    public class AmbientMusic : MonoBehaviour
    {
        //[SerializeField] private AssetReference _soundtrack;
        private IAudioService _audioService;

        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        private void Start()
        {
            //_audioService.SetAmbient(_soundtrack);
        }
    }
}