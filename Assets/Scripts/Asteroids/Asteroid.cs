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

    #endregion Fields

    #region Unity Methods

    private void Start()
    {
        switch (Type)
        {
            case AsteroidType.BigAsteroid:
                MeshR.material.SetColor("_Color", GameStateManager.Instance.Settings.Asteroid.BigAsteroidColor);
                break;

            case AsteroidType.MediumAsteroid:
                MeshR.material.SetColor("_Color", GameStateManager.Instance.Settings.Asteroid.MediumAsteroidColor);
                break;

            case AsteroidType.SmallAsteroid:
                MeshR.material.SetColor("_Color", GameStateManager.Instance.Settings.Asteroid.SmallAsteroidColor);
                break;
        }
    }

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * GameStateManager.Instance.Settings.Asteroid.AsteroidSpeed;
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
        Collider.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        AsteroidsManager.Instance.DestroyAsteroid(this);
        Player.Instance.AddScore();
        GameStateManager.Instance.ScoreUI.text = Player.Instance.Points.ToString();
    }

    public bool CreateExplosion() => true;

    #endregion Methods
}