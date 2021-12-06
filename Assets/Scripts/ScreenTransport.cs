using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ScreenTransport : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;

    public void SetCamera(Camera camera) => Camera = camera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null || other.transform.parent.name != "Floor")
            return;

        Vector3 screenPosition = Camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0)
            screenPosition.x = Screen.width;

        if (screenPosition.y < 0)
            screenPosition.y = Screen.height;

        if (Screen.width < screenPosition.x)
            screenPosition.x = 0;

        if (Screen.height < screenPosition.y)
            screenPosition.y = 0;

        Vector3 newPosition = Camera.ScreenToWorldPoint(screenPosition);
        newPosition.y = 0;
        transform.position = newPosition;
    }
}