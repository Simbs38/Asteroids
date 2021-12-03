using UnityEngine;

public class ResetUIButton : MonoBehaviour
{
    [SerializeField]
    private HighScoreUI ItemToUpdate;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) == 0)
            gameObject.SetActive(false);
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        ItemToUpdate.UpdateScore();
    }
}