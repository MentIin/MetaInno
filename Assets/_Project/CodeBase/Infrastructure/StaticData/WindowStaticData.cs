using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.StaticData
{
    [Serializable]
    [CreateAssetMenu(fileName = "WindowsStaticData", menuName = "StaticData/WindowsStaticData", order = 0)]
    public class WindowsStaticData : ScriptableObject
    {
        public List<WindowConfig> WindowConfigs;
    }

    public class WindowConfig
    {
        public WindowType Type;
        //TODO: replace with asset reference
        public GameObject WindowReference;
    }

    public enum WindowType
    {
        None,
        MainMenu,
        Hud,
    }
}