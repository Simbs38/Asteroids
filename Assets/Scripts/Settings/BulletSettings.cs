using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BulletSettings")]
public class BulletSettings : ScriptableObject
{
    [Header("Bullet Settings")]
    public float Speed = 0.1f;
    public float ShottingDistance = 3;
}