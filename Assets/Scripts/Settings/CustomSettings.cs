using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Settings")]
public class CustomSettings : ScriptableObject
{
    public PlayerSettings Player;

    [Space(10)]

    public BulletSettings Bullet;

    [Space(10)]

    public AsteroidSettings Asteroid;

    [Space(10)]

    public UISettings UI;
}