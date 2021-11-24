using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsManager : MonoBehaviour
{
    public float MinWaitTime = 1;
    public float MaxWaitTime = 5;
    public ShipMovement Player;
    public Asteroid AsteroidPrefab;

    void Start()
    {
        StartCoroutine(ShootAsteroids());
    }

    private IEnumerator ShootAsteroids()
    {
        while (true)
        {
            float waitTime = Random.Range(MinWaitTime, MaxWaitTime);

            yield return new WaitForSeconds(waitTime);
            CreateAsteroid();
        }
    }


    private void CreateAsteroid()
    {
        float positionX = Random.Range(0, Screen.width);
        float positionY = Random.Range(0, Screen.height);
        int edgeXAxis = (int)Random.Range(0,4);

        if(edgeXAxis < 2)
            positionX = edgeXAxis < 1 ? 0 : Screen.width;
        else
            positionY = edgeXAxis < 3 ? 0 : Screen.height;

        Vector3 screenPosition = new Vector3(positionX, positionY, Camera.main.transform.position.y);
        Asteroid tmp = GameObject.Instantiate(AsteroidPrefab);
        tmp.transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        tmp.Direction = (Player.transform.position - tmp.transform.position).normalized;
    }
}
