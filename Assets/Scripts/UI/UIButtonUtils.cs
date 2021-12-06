using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonUtils : MonoBehaviour
{
    public void GoToMainMenu() => SceneManager.LoadScene(0);

    public void GoToGamePlay() => SceneManager.LoadScene(1);
}