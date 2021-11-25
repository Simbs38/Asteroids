using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction;
    public AudioSource BulletSound{
        get {
            if(_bulletSound == null)
                _bulletSound = GetComponent<AudioSource>();

            return _bulletSound;
        }
    }
    private AudioSource _bulletSound;



    private void Update() => transform.position += Direction * Main.Instance.Settings.BulletSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShipMovement>() != null)
        {
            Main.Instance.HitTaken();
            Destroy(gameObject);

            return;
        }

        Asteroid asteroid = other.GetComponent<Asteroid>();

        if (asteroid != null)
            Main.Instance.HitAsteroid(asteroid);

        Destroy(gameObject);
    }

    public void PlaySound() => BulletSound.Play();
}