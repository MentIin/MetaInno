using CodeBase._Project.CodeBase.Infrastructure.Data;

namespace Assets.CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        public void UpdateProgress(PlayerProgress progress);
    }
}