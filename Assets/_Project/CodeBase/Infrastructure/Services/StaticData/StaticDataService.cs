using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<WindowType, WindowConfig> _windowData;

        private static readonly string LevelsStaticData = "StaticData/Levels/AllLevelsStaticData";
        
        public void Load()
        {
            LoadWindowsData();

        }
        private void LoadWindowsData()
        {
            _windowData = Resources.Load<WindowsStaticData>("WindowsStaticData").
                WindowConfigs.ToDictionary(x => x.Type, x => x);
        }
        public WindowConfig ForWindow(WindowType type)
            => _windowData.TryGetValue(type, out WindowConfig config)
                ? config
                : null;
        
    }
}