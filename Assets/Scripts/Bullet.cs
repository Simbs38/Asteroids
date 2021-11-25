using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bullet : MonoBehaviour
{
    public Vector3 Direction;

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

        if (other.GetComponent<ShipMovement>() != null)
            Main.Instance.HitTaken();
        else
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
                Main.Instance.HitAsteroid(asteroid);
        }

        _used = true;
        StartCoroutine(LateDestroy());
    }

    public void PlaySound() => BulletSound.Play();

    private IEnumerator LateDestroy()
    {
        MeshR.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}