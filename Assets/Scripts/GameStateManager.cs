using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    #region Fields

    public static GameStateManager Instance;
    public bool IsGameRunning { get; private set; }
    public CustomSettings Settings;
    public Camera Camera;
    public Text ScoreUI;

    [SerializeField]
    private AudioSource ReSpawnSound;

    [SerializeField]
    private Canvas HitTakenCanvas;

    [SerializeField]
    private Canvas EndGameCanvas;

    #endregion Fields

    #region UnityMethods

    private void Awake() => Instance = this;

    private void Start() => IsGameRunning = true;

    #endregion UnityMethods

    #region Methods

    public void PauseGame()
    {
        HitTakenCanvas.gameObject.SetActive(true);
        IsGameRunning = false;
        AsteroidsManager.Instance.DestroyAsteroids();
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