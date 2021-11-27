using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bullet : MonoBehaviour
{
    #region Fields

    public ParticleSystem ParticleEffect;
    public ParticleSystem BulletDrag;

    public MeshRenderer MeshR
    {
        get
        {
            if (_meshR == null)
                _meshR = GetComponent<MeshRenderer>();

            return _meshR;
        }
    }

    public AudioSource BulletSound
    {
        get
        {
            if (_bulletSound == null)
                _bulletSound = GetComponent<AudioSource>();

            return _bulletSound;
        }
    }

    private bool _used = false;

    private AudioSource _bulletSound;
    private MeshRenderer _meshR;

    #endregion Fields

    #region Unity Methods

    private void Update() => transform.position += transform.forward * Main.Instance.Settings.Bullet.Speed;

    private void OnTriggerEnter(Collider other)
    {
        if (_used)
            return;

        _used = true;

        if (other.GetComponent<Player>() != null)
            Main.Instance.HitTaken();
        else
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                StartCoroutine(CreateExplosion(asteroid.transform.position));
                Main.Instance.HitAsteroid(asteroid);
            }
        }

        StartCoroutine(LateDestroy());
    }

    #endregion Unity Methods

    #region Methods

    public void Shooting(Vector3 position, Vector3 direction)
    {
        BulletSound.Play();
        transform.position = position;
        transform.forward = direction;
    }

    private IEnumerator LateDestroy()
    {
        BulletDrag.gameObject.SetActive(false);
        MeshR.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private IEnumerator CreateExplosion(Vector3 position)
    {
        ParticleSystem tmp = Instantiate(ParticleEffect);
        tmp.transform.position = position;
        tmp.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Destroy(tmp.gameObject);
    }

    #endregion Methods
}