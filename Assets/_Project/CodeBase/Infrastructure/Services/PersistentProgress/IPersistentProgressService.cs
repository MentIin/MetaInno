using CodeBase._Project.CodeBase.Infrastructure.Data;

namespace Assets.CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}