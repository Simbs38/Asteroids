using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonUtils : MonoBehaviour
{
    public GameObject HideOnClick;

    public void GoToMainMenu() => SceneManager.LoadScene(0);

    public void GoToGamePlay() => SceneManager.LoadScene(1);

    public void HideObject() => HideOnClick.SetActive(!HideOnClick.activeSelf);
}