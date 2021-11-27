using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Settings")]
public class CustomSettings : ScriptableObject
{
    [Header("Player Settings")]

    public int StartingHealt = 4;
    public int HealtIconsSize = 5;
    public Color PlayerColor;

    [Space(10)]

    public float MoveSpeed = 20f;
    public float RotateSpeed = 160f;

    [Header("Bullet Settings")]

    public float BulletSpeed = 0.5f;
    public float BulletShottingDistance = 3;
    public float DoubleClickTime = 0.5f;


    [Header("AsteroidManager Settings")]

    public float MinWaitTime = 1;
    public float MaxWaitTime = 5;

    public float AsteroidSpeed = 1;
    public int HitPoints = 1;

    [Space(10)]


    public bool GenerateRandomSizeAsteroids = true;

    public Color BigAsteroidColor;
    public Color MediumAsteroidColor;
    public Color SmallAsteroidColor;


    [Header("UI Settings")]

    public Vector2 OffSet = new Vector2(20, 10);
    public int SpaceBetweenShips = 25;
    public Color BackGroundColor;
}