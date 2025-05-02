using UnityEngine;

namespace CodeBase.DebugLogic
{
    public class DebugSinglton : MonoBehaviour
    {
        public bool LargeFinish = false;
        public bool InfinityResources = false;
        
        public static DebugSinglton Instance;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}