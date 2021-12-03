using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    public Text TextField;
    public bool ExtensiveWrite;

    private void Start() => UpdateScore();

    public void UpdateScore()
    {
        string score = PlayerPrefs.GetInt("HighScore", 0).ToString();
        TextField.text = ExtensiveWrite ? "HighScore: " + score : score;
    }
}