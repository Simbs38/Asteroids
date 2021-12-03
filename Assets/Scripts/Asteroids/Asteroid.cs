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

    public MeshRenderer MeshR
    {
        get
        {
            if (_meshR == null)
                _meshR = GetComponent<MeshRenderer>();

            return _meshR;
        }
    }

    public AudioSource Sound
    {
        get
        {
            if (_sound == null)
                _sound = GetComponent<AudioSource>();

            return _sound;
        }
    }

    private AudioSource _sound;
    private MeshRenderer _meshR;

    #endregion Fields

    #region Unity Methods

    private void Start()
    {
        MeshRenderer meshR = transform.GetComponent<MeshRenderer>();

        switch (Type)
        {
            case AsteroidType.BigAsteroid:
                meshR.material.SetColor("_Color", Main.Instance.Settings.Asteroid.BigAsteroidColor);
                break;

            case AsteroidType.MediumAsteroid:
                meshR.material.SetColor("_Color", Main.Instance.Settings.Asteroid.MediumAsteroidColor);
                break;

            case AsteroidType.SmallAsteroid:
                meshR.material.SetColor("_Color", Main.Instance.Settings.Asteroid.SmallAsteroidColor);
                break;
        }
    }

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * Main.Instance.Settings.Asteroid.AsteroidSpeed;
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Player.Instance.TakeDamage();
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

        if (!forceDestoy && AsteroidToGenerate != null && Player.Instance.Health != 0)
        {
            Vector3 directionA = Quaternion.Euler(0, 30, 0) * Direction;
            Vector3 directionB = Quaternion.Euler(0, -30, 0) * Direction;

            AsteroidsManager.Instance.CreateReplicas(AsteroidToGenerate, transform.position, directionA);
            AsteroidsManager.Instance.CreateReplicas(AsteroidToGenerate, transform.position, directionB);
        }
    }

    private IEnumerator LateDestroy()
    {
        if (Sound.isActiveAndEnabled)
            Sound.Play();

        MeshR.enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    #endregion Methods

    public void TakeDamage()
    {
        AsteroidsManager.Instance.DestroyAsteroid(this);
        Player.Instance.AddScore();
        Main.Instance.ScoreUI.text = Player.Instance.Points.ToString();
    }

    public bool CreateExplosion() => true;
}