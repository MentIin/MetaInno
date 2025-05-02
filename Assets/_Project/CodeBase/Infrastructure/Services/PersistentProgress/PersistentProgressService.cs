using CodeBase._Project.CodeBase.Infrastructure.Data;

namespace Assets.CodeBase.Infrastructure.Services.PersistentProgress
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