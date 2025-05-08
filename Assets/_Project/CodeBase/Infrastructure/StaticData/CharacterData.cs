using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "StaticData/CharacterData")]
public class CharacterData : ScriptableObject
{
    public float slopeLimit = 45f;
    public float stepOffset = 0.3f;
    public float skinWidth = 0.08f;
    public float minMoveDistance = 0.001f;
    public Vector3 center;
    public float radius = 0.5f;
    public float height = 2f;
}
