using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
    [Header("Player Settings")]
    public int StartingHealt = 4;

    public int HealtIconsSize = 5;
    public Color PlayerColor;

    [Header("Move Settings")]
    public float MoveSpeed = 20f;

    public float RotateSpeed = 160f;
}