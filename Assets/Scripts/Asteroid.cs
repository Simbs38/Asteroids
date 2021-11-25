using UnityEngine;

[RequireComponent(typeof(ScreenTransport))]
public class Asteroid : MonoBehaviour
{
    public Vector3 Direction;
    public float AsteroidSpeed = 1;
    public int Level;

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * AsteroidSpeed;
        transform.rotation = Quaternion.identity;
    }

    public void Dispose() => Destroy(gameObject);

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShipMovement>() != null)
        {
            Main.Instance.HitTaken();
            Dispose();
        }
    }
}