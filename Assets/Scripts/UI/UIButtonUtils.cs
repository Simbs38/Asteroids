using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonUtils : MonoBehaviour
{
    #region Fields

    public GameObject HideOnClick;

    #endregion Fields

    #region Methods

    public void GoToMainMenu() => SceneManager.LoadScene(0);

    public void GoToGamePlay() => SceneManager.LoadScene(1);

    public void HideObject() => HideOnClick.SetActive(!HideOnClick.activeSelf);

    #endregion Methods
}