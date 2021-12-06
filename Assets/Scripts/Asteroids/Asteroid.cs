using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ScreenTransport))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
public class Asteroid : MonoBehaviour, IDamageable
{
    #region Fields

    public Vector3 Direction;
    public AsteroidType Type;
    public Asteroid AsteroidToGenerate;

    [SerializeField]
    private MeshRenderer MeshR;

    [SerializeField]
    private AudioSource Sound;

    [SerializeField]
    private Collider Collider;

    [SerializeField]
    private ScreenTransport ScreenTransporter;

    private float _speed;
    private AsteroidsManager _asteroidManager;

    #endregion Fields

    #region Unity Methods

    public void InitAsteroid(float speed, Color color, Camera camera, AsteroidsManager manager)
    {
        _speed = speed;
        _asteroidManager = manager;
        MeshR.material.SetColor("_Color", color);
        ScreenTransporter.SetCamera(camera);
    }

    public void SetColor(Color color) => MeshR.material.SetColor("_Color", color);

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * _speed;
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().TakeDamage();
            Dispose(forceDestoy: true);
        }
    }

    #endregion Unity Methods

    #region Methods

    public void Dispose(bool forceDestoy = false)
    {
        if (forceDestoy)
        {
            Sound.Stop();
            Destroy(gameObject);
        }
        else
            StartCoroutine(LateDestroy());

        if (!forceDestoy && AsteroidToGenerate != null)
        {
            Vector3 directionA = Quaternion.Euler(0, 30, 0) * Direction;
            Vector3 directionB = Quaternion.Euler(0, -30, 0) * Direction;

            _asteroidManager.CreateReplicas(AsteroidToGenerate, transform.position, directionA);
            _asteroidManager.CreateReplicas(AsteroidToGenerate, transform.position, directionB);
        }
    }

    private IEnumerator LateDestroy()
    {
        if (Sound.isActiveAndEnabled)
            Sound.Play();

        MeshR.enabled = false;
        Collider.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void TakeDamage() => _asteroidManager.DestroyAsteroid(this);

    public bool CreateExplosion() => true;

    public bool IsSucessfullHit(out int hitPoints)
    {
        hitPoints = _asteroidManager.HitPoints;
        return true;
    }

    #endregion Methods
}