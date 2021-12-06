using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bullet : MonoBehaviour
{
    public float ShootingDistance => Settings.ShottingDistance;

    [SerializeField]
    private ParticleSystem ParticleEffect;
    [SerializeField]
    private ParticleSystem BulletDrag;
    [SerializeField]
    private MeshRenderer MeshR;
    [SerializeField]
    private AudioSource BulletSound;
    [SerializeField]
    private BulletSettings Settings;
    [SerializeField]
    private Player Player;
    private bool _used = false;

    private void Update() => transform.position += transform.forward * Settings.Speed;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable tmp = other.GetComponent<IDamageable>();

        if (tmp == null || _used)
            return;

        _used = true;

        tmp.TakeDamage();

        if(tmp.CreateExplosion())
            StartCoroutine(CreateExplosion(other.transform.position));

        if (tmp.IsSucessfullHit(out int hitPoints))
            Player.AddScore(hitPoints);

        StartCoroutine(LateDestroy());
    }

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
}