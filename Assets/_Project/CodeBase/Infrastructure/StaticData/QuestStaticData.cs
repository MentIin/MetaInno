using UnityEngine;

namespace CodeBase.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "NewQuestData", menuName = "StaticData/QuestData")]
    public class QuestStaticData : ScriptableObject
    {
        public int Id;
        public string Title;
        public string Description;

        public float Time = 30f;
    }
}