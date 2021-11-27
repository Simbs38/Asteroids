using UnityEngine;

public class ResetUIButton : MonoBehaviour
{
    #region Fields

    public HighScoreUI ItemToUpdate;

    #endregion Fields

    #region Unity Methods

    private void Awake()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) == 0)
            gameObject.SetActive(false);
    }

    #endregion Unity Methods

    #region Methods

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        ItemToUpdate.UpdateScore();
    }

    #endregion Methods
}