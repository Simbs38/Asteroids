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

        IDamageable tmp = other.GetComponent<IDamageable>();

        if (tmp == null)
            return;

        _used = true;

        tmp.TakeDamage();

        if(tmp.CreateExplosion())
            StartCoroutine(CreateExplosion(other.transform.position));

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