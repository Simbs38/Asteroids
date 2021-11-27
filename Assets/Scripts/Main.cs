using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    #region Fields

    public static Main Instance;
    public bool IsGameRunning { get; private set; }

    public AudioSource ReSpawnSound;
    public CustomSettings Settings;
    public Canvas HitTakenCanvas;
    public Canvas EndGameCanvas;
    public Player Player;
    public Camera Camera;
    public Text ScoreUI;

    #endregion Fields

    #region UnityMethods

    private void Awake() => Instance = this;

    private void Start()
    {
        IsGameRunning = true;
        HealtUIManager.Instance.PopulateHealtUI(Settings.Player.StartingHealt);
    }

    #endregion UnityMethods

    #region Methods

    public void HitTaken()
    {
        Player.TakeHit();
        HealtUIManager.Instance.RemoveHealt();

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

    public void HitAsteroid(Asteroid asteroid)
    {
        AsteroidsManager.Instance.DestroyAsteroid(asteroid);
        Player.AddScore();
        ScoreUI.text = Player.Points.ToString();
    }

    private IEnumerator StopGame()
    {
        ReSpawnSound.Play();
        yield return new WaitForSeconds(2);
        HitTakenCanvas.gameObject.SetActive(false);
        IsGameRunning = true;
    }

    private void EndGame(int playerPoints)
    {
        IsGameRunning = false;
        EndGameCanvas.gameObject.SetActive(true);
        PlayerPrefs.SetInt("HighScore", Mathf.Max(playerPoints, PlayerPrefs.GetInt("HighScore", 0)));
    }

    #endregion Methods
}