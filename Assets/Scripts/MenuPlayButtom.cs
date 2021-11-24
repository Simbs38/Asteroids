using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayButtom : MonoBehaviour
{
    public void GoToGamePlay() => SceneManager.LoadScene(1);
}