using UnityEngine;

public class ScreenTransport : MonoBehaviour
{
    private void OnTriggerEnter()
    {
        Camera camera = Camera.main;
        Vector3 screenPosition = camera.WorldToScreenPoint(transform.position);

        if(screenPosition.x < 0)
            screenPosition.x = Screen.width;

        if(screenPosition.y < 0)
            screenPosition.y = Screen.height;

        if(Screen.width < screenPosition.x)
            screenPosition.x = 0;

        if(Screen.height < screenPosition.y)
            screenPosition.y = 0;

        transform.position = camera.ScreenToWorldPoint(screenPosition);
    }
}