using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShipMovement : MonoBehaviour
{
    public Bullet BulletPrefab;
    public Rigidbody RBody;
    public ParticleSystem LeftMotor;
    public ParticleSystem RigthMotor;
    public int Points { get; private set; }
    public int Health { get; private set; }

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

    private void Start()
    {
        Health = Main.Instance.Settings.StartingHealt;
        Points = 0;
        MeshRenderer meshR = transform.GetComponentInChildren<MeshRenderer>();
        meshR.material.color = Main.Instance.Settings.PlayerColor;
    }

    private void FixedUpdate()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float v = Input.GetAxisRaw("Vertical");
        float rotation = Input.GetAxisRaw("Horizontal");

        SetMotor(LeftMotor, 0 < v, Mathf.Sign(rotation) == 1 && rotation != 0);
        SetMotor(RigthMotor, 0 < v, Mathf.Sign(rotation) == -1 && rotation != 0);

        RBody.AddForce((transform.forward * v * Main.Instance.Settings.MoveSpeed) - RBody.velocity, ForceMode.Force);
    }

    private void SetMotor(ParticleSystem motor, bool movingForward, bool movingToThiSide)
    {
        if(!motor.isPlaying && (movingForward || movingToThiSide))
            motor.Play();

        else if(motor.isPlaying && !movingForward && !movingToThiSide)
            motor.Stop();
    }

    private void Update()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float rotation = Input.GetAxisRaw("Horizontal");
        Vector3 currentEulerRotation = transform.eulerAngles;
        currentEulerRotation.y += rotation * Time.smoothDeltaTime * Main.Instance.Settings.RotateSpeed;
        transform.localEulerAngles = currentEulerRotation;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    public void UpdateScore()
    {
        Points += Main.Instance.Settings.HitPoints;
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(BulletPrefab);
        bullet.PlaySound();
        bullet.Direction = transform.forward;
        bullet.transform.forward = transform.forward;
        bullet.transform.position = transform.position + transform.forward * Main.Instance.Settings.BulletShottingDistance;
    }

    public void TakeHit()
    {
        Sound.Play();
        Health--;
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        RBody.velocity = Vector3.zero;
        RBody.angularVelocity = Vector3.zero;

        if(LeftMotor.isPlaying)
            LeftMotor.Stop();
        if(RigthMotor.isPlaying)
            RigthMotor.Stop();
    }
}