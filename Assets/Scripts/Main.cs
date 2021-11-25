using System.Collections;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;

    public bool IsGameRunning { get; private set; }
    public ShipMovement Player;
    public Canvas HitTakenCanvas;

    private void Awake() => Instance = this;

    private void Start() => IsGameRunning = true;

    public void HitTaken()
    {
        HitTakenCanvas.gameObject.SetActive(true);
        IsGameRunning = false;
        AsteroidsManager.Instance.DestroyAsteroids();
        Player.TakeHit();
        StartCoroutine(StopGame());
    }

    public IEnumerator StopGame()
    {
        yield return new WaitForSeconds(2);
        HitTakenCanvas.gameObject.SetActive(false);
        IsGameRunning = true;
        Player.ResetPosition();
    }


    public void HitAsteroid(Asteroid asteroid)
    {
        AsteroidsManager.Instance.DestroyAsteroid(asteroid);
        Player.UpdateScore();
    }
}