using UnityEngine;

[RequireComponent(typeof(ScreenTransport))]
public class Asteroid : MonoBehaviour
{
    public Vector3 Direction;
    public AsteroidType Type;
    public Asteroid AsteroidToGenerate;

    private void Start()
    {
        MeshRenderer meshR = transform.GetComponentInChildren<MeshRenderer>();

        switch (Type)
        {
            case AsteroidType.BigAsteroid:
                meshR.material.color = Main.Instance.Settings.BigAsteroidColor;
                break;

            case AsteroidType.MediumAsteroid:
                meshR.material.color = Main.Instance.Settings.MediumAsteroidColor;
                break;

            case AsteroidType.SmallAsteroid:
                meshR.material.color = Main.Instance.Settings.SmallAsteroidColor;
                break;
        }
    }

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * Main.Instance.Settings.AsteroidSpeed;
        transform.rotation = Quaternion.identity;
    }

    public void Dispose(bool forceDestoy = false)
    {
        Destroy(gameObject);

        if (!forceDestoy && AsteroidToGenerate != null)
        {
            Vector3 directionA = Quaternion.Euler(0, 30, 0) * Direction;
            Vector3 directionB = Quaternion.Euler(0, -30, 0) * Direction;

            AsteroidsManager.Instance.CreateReplicas(AsteroidToGenerate, transform.position, directionA);
            AsteroidsManager.Instance.CreateReplicas(AsteroidToGenerate, transform.position, directionB);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShipMovement>() != null)
        {
            Main.Instance.HitTaken();
            Dispose();
        }
    }
}