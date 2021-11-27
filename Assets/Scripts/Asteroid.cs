using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ScreenTransport))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
public class Asteroid : MonoBehaviour
{
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

    private void Start()
    {
        MeshRenderer meshR = transform.GetComponent<MeshRenderer>();

        switch (Type)
        {
            case AsteroidType.BigAsteroid:
                meshR.material.SetColor("_Color", Main.Instance.Settings.BigAsteroidColor);
                break;

            case AsteroidType.MediumAsteroid:
                meshR.material.SetColor("_Color", Main.Instance.Settings.MediumAsteroidColor);
                break;

            case AsteroidType.SmallAsteroid:
                meshR.material.SetColor("_Color", Main.Instance.Settings.SmallAsteroidColor);
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
        if (forceDestoy)
        {
            Sound.Stop();
            Destroy(gameObject);
        }
        else
            StartCoroutine(LateDestroy());

        if (!forceDestoy && AsteroidToGenerate != null && Main.Instance.Player.Health != 0)
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
            Dispose(forceDestoy:true);
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
}