using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.AudioService
{
    public interface IAudioService : IService
    {
        Task SetAmbient(AudioClip ambient);
        void ClearAmbient();
        void PlaySound(AudioClip sound, Vector3 pos);
        void PlaySound(AudioClip sound,Vector3 pos, bool randomize);
        LoopingSound CreateLoopingSound(AudioClip sound);
        void PlaySound(AudioClip sound, Vector3 pos, bool randomize, float volume, bool pause, bool ignorePause);
        void UpdateVolume();
        void PauseSounds();
        void ResumeSounds();
        LoopingSound CreateLoopingSound(AudioClip sound, bool ignorePosition);
        void PauseAmbient();
        void ResumeAmbient();
    }
}