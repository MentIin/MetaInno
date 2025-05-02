using CodeBase._Project.CodeBase.Infrastructure.Data;

namespace Assets.CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        public void SaveProgress();
        public PlayerProgress LoadProgress();
    }
}