using UnityEngine;

namespace CodeBase.DebugLogic
{
    public class DebugSingleton : MonoBehaviour
    {
        public bool InfiniteMoney=false;
        
        public static DebugSingleton Instance;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}