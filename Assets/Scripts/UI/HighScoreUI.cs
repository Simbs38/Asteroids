using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    #region Fields

    public Text TextField;
    public bool ExtensiveWrite;

    #endregion Fields

    #region Unity Methods

    private void Start() => UpdateScore();

    #endregion Unity Methods

    #region Methods

    public void UpdateScore()
    {
        string score = PlayerPrefs.GetInt("HighScore", 0).ToString();
        TextField.text = ExtensiveWrite ? "HighScore: " + score : score;
    }

    #endregion Methods
}