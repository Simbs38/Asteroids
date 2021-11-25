using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static Main Instance;

    public bool IsGameRunning { get; private set; }
    public ShipMovement Player;
    public Canvas HitTakenCanvas;
    public Canvas EndGameCanvas;
    public Text ScoreUI;

    private void Awake() => Instance = this;

    private void Start()
    {
        IsGameRunning = true;
        HealtManager.Instance.PopulateHealtUI(Player.StartingHealt);
    }

    public void HitTaken()
    {
        Player.TakeHit();
        HealtManager.Instance.RemoveHealt();

        if (Player.Health == 0)
            EndGame();
        else
        {
            HitTakenCanvas.gameObject.SetActive(true);
            IsGameRunning = false;
            AsteroidsManager.Instance.DestroyAsteroids();
            StartCoroutine(StopGame());
        }
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

    public void EndGame()
    {
        IsGameRunning = false;
        EndGameCanvas.gameObject.SetActive(true);
    }
}