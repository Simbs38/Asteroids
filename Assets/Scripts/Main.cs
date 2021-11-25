using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public AudioSource ReSpawnSound;

    public bool IsGameRunning { get; private set; }
    public ShipMovement Player;
    public Canvas HitTakenCanvas;
    public Canvas EndGameCanvas;
    public Text ScoreUI;
    public CustomSettings Settings;

    private void Awake() => Instance = this;

    private void Start()
    {
        IsGameRunning = true;
        HealtManager.Instance.PopulateHealtUI(Settings.StartingHealt);
        Camera.main.backgroundColor = Main.Instance.Settings.BackGroundColor;
    }

    public void HitTaken()
    {
        Player.TakeHit();
        HealtManager.Instance.RemoveHealt();

        if (Player.Health == 0)
            EndGame(Player.Points);
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
        ReSpawnSound.Play();
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

    public void EndGame(int playerPoints)
    {
        IsGameRunning = false;
        EndGameCanvas.gameObject.SetActive(true);
        PlayerPrefs.SetInt("HighScore", Mathf.Max(playerPoints, PlayerPrefs.GetInt("HighScore",0)));
    }
}