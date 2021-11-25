using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidType
{
    BigAsteroid = 1,
    MediumAsteroid = 2,
    SmallAsteroid = 3
}

public class AsteroidsManager : MonoBehaviour
{
    public static AsteroidsManager Instance;

    public ShipMovement Player;
    public Asteroid AsteroidBig;
    public Asteroid AsteroidMedium;
    public Asteroid AsteroidSmall;
    public List<Asteroid> _existingAsteroids;

    private void Awake() => Instance = this;

    private void Start()
    {
        StartCoroutine(ShootAsteroids());
        _existingAsteroids = new List<Asteroid>();
    }

    public void DestroyAsteroids()
    {
        while (_existingAsteroids.Count != 0)
            DestroyAsteroid(_existingAsteroids[0], force: true);

        _existingAsteroids = new List<Asteroid>();
    }

    public void DestroyAsteroid(Asteroid asteroid, bool force = false)
    {
        if (_existingAsteroids.Contains(asteroid))
            _existingAsteroids.Remove(asteroid);

        asteroid.Dispose(force);
    }

    public void CreateReplicas(Asteroid asteroidPrefab, Vector3 position, Vector3 direction)
    {
        Asteroid tmp = Instantiate(asteroidPrefab);
        tmp.transform.position = position;
        tmp.Direction = direction;
        _existingAsteroids.Add(tmp);
    }

    private IEnumerator ShootAsteroids()
    {
        while (true)
        {
            float waitTime = Random.Range(Main.Instance.Settings.MinWaitTime, Main.Instance.Settings.MaxWaitTime);

            yield return new WaitForSeconds(waitTime);

            if (Main.Instance.IsGameRunning)
                _existingAsteroids.Add(CreateAsteroid());
        }
    }

    private Asteroid CreateAsteroid()
    {
        float positionX = Random.Range(0, Screen.width);
        float positionY = Random.Range(0, Screen.height);
        int edgeXAxis = (int)Random.Range(0, 4);

        if (edgeXAxis < 2)
            positionX = edgeXAxis < 1 ? 0 : Screen.width;
        else
            positionY = edgeXAxis < 3 ? 0 : Screen.height;

        Vector3 screenPosition = new Vector3(positionX, positionY, Camera.main.transform.position.y);
        Asteroid prefab = AsteroidBig;

        if (Main.Instance.Settings.GenerateRandomSizeAsteroids)
        {
            int option = Random.Range(0, 3);

            switch (option)
            {
                case 0:
                    prefab = AsteroidSmall;
                    break;
                case 1:
                    prefab = AsteroidMedium;
                    break;
            }
        }

        Asteroid tmp = Instantiate(prefab);
        tmp.transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        tmp.Direction = (Player.transform.position - tmp.transform.position).normalized;

        return tmp;
    }
}