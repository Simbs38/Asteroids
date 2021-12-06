using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    #region Fields

    public bool IsGameRunning { get; private set; }
    public Text ScoreUI;

    [SerializeField]
    private AudioSource ReSpawnSound;

    [SerializeField]
    private Canvas HitTakenCanvas;

    [SerializeField]
    private Canvas EndGameCanvas;
    [SerializeField]
    private AsteroidsManager AsteroidsManager;

    #endregion Fields

    #region UnityMethods

    private void Start() => IsGameRunning = true;

    #endregion UnityMethods

    #region Methods

    public void PauseGame()
    {
        HitTakenCanvas.gameObject.SetActive(true);
        IsGameRunning = false;
        AsteroidsManager.DestroyAsteroids();
        StartCoroutine(StopGame());
    }

    public void EndGame(int playerPoints)
    {
        IsGameRunning = false;
        EndGameCanvas.gameObject.SetActive(true);
        PlayerPrefs.SetInt("HighScore", Mathf.Max(playerPoints, PlayerPrefs.GetInt("HighScore", 0)));
    }

    private IEnumerator StopGame()
    {
        ReSpawnSound.Play();
        yield return new WaitForSeconds(2);
        HitTakenCanvas.gameObject.SetActive(false);
        IsGameRunning = true;
    }

    #endregion Methods
}