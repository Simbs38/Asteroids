using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Player Settings")]
    public int StartingHealt = 4;
    public int BulletPoolSize = 20;
    public Color PlayerColor;

    [Header("Move Settings")]
    public float MoveSpeed = 20f;

    public float RotateSpeed = 160f;
}