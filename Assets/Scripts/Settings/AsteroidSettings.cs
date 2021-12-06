using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/AsteroidSettings")]
public class AsteroidSettings : ScriptableObject
{
    [Header("AsteroidManager Settings")]

    public int HitPoints = 1;
    public float AsteroidSpeed = 3;
    public float MinWaitTime = 1;
    public float MaxWaitTime = 5;
    public bool GenerateRandomSizeAsteroids = true;

    [Header("Asteroids Colors")]

    public Color BigAsteroidColor;
    public Color MediumAsteroidColor;
    public Color SmallAsteroidColor;
}