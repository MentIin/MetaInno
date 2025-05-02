using CodeBase.Infrastructure.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }

        public PersistentProgressService()
        {
            Progress = new PlayerProgress();
        }
    }
}