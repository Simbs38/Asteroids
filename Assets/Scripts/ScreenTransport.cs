using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ScreenTransport : MonoBehaviour
{
    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null || other.transform.parent.name != "Floor")
            return;

        Vector3 screenPosition = Main.Instance.Camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0)
            screenPosition.x = Screen.width;

        if (screenPosition.y < 0)
            screenPosition.y = Screen.height;

        if (Screen.width < screenPosition.x)
            screenPosition.x = 0;

        if (Screen.height < screenPosition.y)
            screenPosition.y = 0;

        Vector3 newPosition = Main.Instance.Camera.ScreenToWorldPoint(screenPosition);
        newPosition.y = 0;
        transform.position = newPosition;
    }

    #endregion Unity Methods
}