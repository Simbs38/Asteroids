using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum AsteroidType
{
    BigAsteroid = 1,
    MediumAsteroid = 2,
    SmallAsteroid = 3
}

public class AsteroidsManager : MonoBehaviour
{
    #region Fields

    public int HitPoints => Settings.HitPoints;
    
    [SerializeField]
    private AssetReference AsteroidBig;

    [SerializeField]
    private AssetReference AsteroidMedium;

    [SerializeField]
    private AssetReference AsteroidSmall;

    [SerializeField]
    private AsteroidSettings Settings;

    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private GameStateManager Manager;

    private List<Asteroid> _existingAsteroids;

    #endregion Fields

    #region Unity Methods

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
        tmp.InitAsteroid(Settings.AsteroidSpeed, GetAsteroidColor(tmp.Type), Camera, this);
        tmp.transform.position = position;
        tmp.Direction = direction;
        _existingAsteroids.Add(tmp);
    }

    private IEnumerator ShootAsteroids()
    {
        while (true)
        {
            float waitTime = Random.Range(Settings.MinWaitTime, Settings.MaxWaitTime);

            yield return new WaitForSeconds(waitTime);

            if (Manager.IsGameRunning)
                CreateAsteroid();
        }
    }

    private void CreateAsteroid()
    {
        float positionX = Random.Range(0, Screen.width);
        float positionY = Random.Range(0, Screen.height);
        GetScreenEdgePosition(ref positionX, ref positionY);

        Vector3 screenPosition = new Vector3(positionX, positionY, Camera.transform.position.y);
        AssetReference asteroidToClone = GetAsteroidPrefabToGenerate();

        asteroidToClone.InstantiateAsync(Vector3.one * 1000, Quaternion.identity).Completed += (ans) => {
            Asteroid ast = ans.Result.GetComponent<Asteroid>();
            ast.InitAsteroid(Settings.AsteroidSpeed, GetAsteroidColor(ast.Type), Camera, this);

            ast.transform.position = Camera.ScreenToWorldPoint(screenPosition);
            ast.Direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _existingAsteroids.Add(ast);
        };
    }

    private void GetScreenEdgePosition(ref float positionX, ref float positionY)
    {
        int edgeXAxis = (int)Random.Range(0, 4);

        if (edgeXAxis < 2)
            positionX = edgeXAxis < 1 ? 0 : Screen.width;
        else
            positionY = edgeXAxis < 3 ? 0 : Screen.height;
    }

    private AssetReference GetAsteroidPrefabToGenerate()
    {
        if (Settings.GenerateRandomSizeAsteroids)
        {
            int option = Random.Range(0, 3);

            switch (option)
            {
                case 0:
                    return AsteroidSmall;

                case 1:
                    return AsteroidMedium;
                default:
                    return AsteroidBig;
            }
        }
        else
            return AsteroidBig;
    }

    private Color GetAsteroidColor(AsteroidType type)
    {
        switch (type)
        {
            case AsteroidType.BigAsteroid:
                return Settings.BigAsteroidColor;

            case AsteroidType.MediumAsteroid:
                return Settings.MediumAsteroidColor;

            case AsteroidType.SmallAsteroid:
                return Settings.SmallAsteroidColor;
        }

        return Color.black;
    }

    #endregion Methods
}