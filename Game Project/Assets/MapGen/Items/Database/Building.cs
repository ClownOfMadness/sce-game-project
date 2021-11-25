using UnityEngine;

[CreateAssetMenu(fileName = "new Building", menuName = "Assets/Building")]
public class Building : ScriptableObject
{
    public string BuildingID;
    public string BuildingName;
    [TextArea]
    public string Description;
    public Sprite BuildingSprite;
}
