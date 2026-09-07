using UnityEngine;

[CreateAssetMenu(fileName = "Tooltip", menuName = "ScriptableObjects/Tooltip")]
public sealed class Tooltip : ScriptableObject
{
    [TextArea(0, 1)] public string title;
    [TextArea(2, 6)] public string description;
    public Sprite sprite;
}
