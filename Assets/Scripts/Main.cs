using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static Main Instance;

    public bool IsGameRunning { get; private set; }
    public ShipMovement Player;
    public Canvas HitTakenCanvas;
    public Text ScoreUI;

    private void Awake() => Instance = this;

    private void Start()
    {
        IsGameRunning = true;
        HealtManager.Instance.PopulateHealtUI(Player.StartingHealt);
    }

    public void HitTaken()
    {
        HitTakenCanvas.gameObject.SetActive(true);
        IsGameRunning = false;
        AsteroidsManager.Instance.DestroyAsteroids();
        Player.TakeHit();
        StartCoroutine(StopGame());
        HealtManager.Instance.RemoveHealt();
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
        ScoreUI.text = Player.Points.ToString();
    }
}