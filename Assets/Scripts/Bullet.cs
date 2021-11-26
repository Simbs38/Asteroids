using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bullet : MonoBehaviour
{
    public Vector3 Direction;
    public ParticleSystem ParticleEffect;

    public MeshRenderer MeshR
    {
        get
        {
            if(_meshR == null)
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

    private void Update() => transform.position += Direction * Main.Instance.Settings.BulletSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if(_used)
            return;

        _used = true;

        if (other.GetComponent<ShipMovement>() != null)
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

    public void PlaySound() => BulletSound.Play();

    private IEnumerator LateDestroy()
    {
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
}