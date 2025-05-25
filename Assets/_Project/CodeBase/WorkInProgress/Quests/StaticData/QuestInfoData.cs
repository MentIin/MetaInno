using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestInfoData", menuName = "StaticData/Quest/Info Data")]
public class QuestInfoData : ScriptableObject
{
    public string ID;
    public string name;
    public string description;
}
