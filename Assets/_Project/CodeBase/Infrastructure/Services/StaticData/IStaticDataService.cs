using Assets.CodeBase.Infrastructure.Services;
using Assets.CodeBase.StaticData;

namespace CodeBase._Project.CodeBase.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        WindowConfig ForWindow(WindowType type);
        
    }
}