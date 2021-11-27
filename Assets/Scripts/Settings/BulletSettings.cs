using System;
using UnityEngine;

[Serializable]
public class BulletSettings
{
    [Header("Bullet Settings")]

    public float Speed = 0.1f;
    public float ShottingDistance = 3;
    public float DoubleClickTime = 0.5f;
}