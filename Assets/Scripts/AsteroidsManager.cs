using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsManager : MonoBehaviour
{
    public static AsteroidsManager Instance;

    public float MinWaitTime = 1;
    public float MaxWaitTime = 5;
    public ShipMovement Player;
    public Asteroid AsteroidPrefab;
    public List<Asteroid> _existingAsteroids;

    private void Awake() => Instance = this;

    private void Start()
    {
        StartCoroutine(ShootAsteroids());
        _existingAsteroids = new List<Asteroid>();
    }

    public void DestroyAsteroids()
    {
        while(_existingAsteroids.Count != 0)
            DestroyAsteroid(_existingAsteroids[0]);

        _existingAsteroids = new List<Asteroid>();
    }

    public void DestroyAsteroid(Asteroid asteroid)
    {
        if(_existingAsteroids.Contains(asteroid))
            _existingAsteroids.Remove(asteroid);

        asteroid.Dispose();
    }

    private IEnumerator ShootAsteroids()
    {
        while (true)
        {
            float waitTime = Random.Range(MinWaitTime, MaxWaitTime);

            yield return new WaitForSeconds(waitTime);

            if(Main.Instance.IsGameRunning)
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
        Asteroid tmp = GameObject.Instantiate(AsteroidPrefab);
        tmp.transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        tmp.Direction = (Player.transform.position - tmp.transform.position).normalized;

        return tmp;
    }
}