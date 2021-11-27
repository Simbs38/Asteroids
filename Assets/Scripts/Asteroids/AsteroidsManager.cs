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
    #region Fields

    public static AsteroidsManager Instance;

    public Player Player;
    public Asteroid AsteroidBig;
    public Asteroid AsteroidMedium;
    public Asteroid AsteroidSmall;
    private List<Asteroid> _existingAsteroids;

    #endregion Fields

    #region Unity Methods

    private void Awake() => Instance = this;

    private void Start()
    {
        StartCoroutine(ShootAsteroids());
        _existingAsteroids = new List<Asteroid>();
    }

    #endregion Unity Methods

    #region Methods

    public void DestroyAsteroids()
    {
        while (_existingAsteroids.Count != 0)
            DestroyAsteroid(_existingAsteroids[0], force: true);

        _existingAsteroids.Clear();
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
            float waitTime = Random.Range(Main.Instance.Settings.Asteroid.MinWaitTime, Main.Instance.Settings.Asteroid.MaxWaitTime);

            yield return new WaitForSeconds(waitTime);

            if (Main.Instance.IsGameRunning)
                _existingAsteroids.Add(CreateAsteroid());
        }
    }

    private Asteroid CreateAsteroid()
    {
        float positionX = Random.Range(0, Screen.width);
        float positionY = Random.Range(0, Screen.height);
        GetScreenEdgePosition(ref positionX, ref positionY);

        Vector3 screenPosition = new Vector3(positionX, positionY, Main.Instance.Camera.transform.position.y);
        Asteroid prefab = GetAsteroidPrefabToGenerate();
        Asteroid tmp = Instantiate(prefab);

        tmp.transform.position = Main.Instance.Camera.ScreenToWorldPoint(screenPosition);
        tmp.Direction = (Player.transform.position - tmp.transform.position).normalized;

        return tmp;
    }

    private void GetScreenEdgePosition(ref float positionX, ref float positionY)
    {
        int edgeXAxis = (int)Random.Range(0, 4);

        if (edgeXAxis < 2)
            positionX = edgeXAxis < 1 ? 0 : Screen.width;
        else
            positionY = edgeXAxis < 3 ? 0 : Screen.height;
    }

    private Asteroid GetAsteroidPrefabToGenerate()
    {
         Asteroid ans = AsteroidBig;

        if (Main.Instance.Settings.Asteroid.GenerateRandomSizeAsteroids)
        {
            int option = Random.Range(0, 3);

            switch (option)
            {
                case 0:
                    ans = AsteroidSmall;
                    break;

                case 1:
                    ans = AsteroidMedium;
                    break;
            }
        }

        return ans;
    }

    #endregion Methods
}