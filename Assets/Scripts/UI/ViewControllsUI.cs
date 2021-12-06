using UnityEngine;

public class ViewControllsUI : MonoBehaviour
{
    public GameObject HideOnClick;

    public void HideObject() => HideOnClick.SetActive(!HideOnClick.activeSelf);
}