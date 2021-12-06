using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UISettings")]
public class UISettings : ScriptableObject
{
    [Header("UI Settings")]
    public Vector2 OffSet = new Vector2(20, 10);

    public int HealtIconsSize = 5;
    public int SpaceBetweenShips = 25;
}