using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    #region Fields

    public int Points { get; private set; }
    public int Health { get; private set; }

    public Bullet BulletPrefab;
    public Rigidbody RBody;
    public ParticleSystem LeftMotor;
    public ParticleSystem RigthMotor;
    public Joystick CurrentJoystick;

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

    #endregion Fields

    #region Unity Methods

    private void Start()
    {
        Health = Main.Instance.Settings.StartingHealt;
        Points = 0;
        MeshRenderer meshR = transform.GetComponent<MeshRenderer>();
        meshR.material.color = Main.Instance.Settings.PlayerColor;
    }

    private void FixedUpdate()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float movement = GetMovement();
        float rotation = GetRotation();

        SetMotor(LeftMotor, 0 < movement, Mathf.Sign(rotation) == 1 && rotation != 0);
        SetMotor(RigthMotor, 0 < movement, Mathf.Sign(rotation) == -1 && rotation != 0);

        RBody.AddForce((transform.forward * movement * Main.Instance.Settings.MoveSpeed) - RBody.velocity, ForceMode.Force);
    }

    private void Update()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float rotation = GetRotation();
        Vector3 currentEulerRotation = transform.eulerAngles;
        currentEulerRotation.y += rotation * Time.smoothDeltaTime * Main.Instance.Settings.RotateSpeed;
        transform.localEulerAngles = currentEulerRotation;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    #endregion Unity Methods

    #region Methods

    public void Shoot()
    {
        Bullet bullet = Instantiate(BulletPrefab);
        Vector3 spwanPosition = transform.position + transform.forward * Main.Instance.Settings.BulletShottingDistance;
        bullet.Shooting(spwanPosition, transform.forward);
    }

    public void AddScore() => Points += Main.Instance.Settings.HitPoints;

    public void TakeHit()
    {
        Sound.Play();
        Health--;
        ResetPosition();
    }

    private float GetRotation()
    {
        float rotation = Input.GetAxisRaw("Horizontal");
        float joystickInput = CurrentJoystick.Horizontal;

        return joystickInput == 0 ? rotation : joystickInput;
    }

    private float GetMovement()
    {
        float movement = Input.GetAxisRaw("Vertical");
        float joystickInput = CurrentJoystick.Vertical;

        return joystickInput == 0 ? movement : joystickInput;
    }

    private void SetMotor(ParticleSystem motor, bool movingForward, bool movingToThiSide)
    {
        if (!motor.isPlaying && (movingForward || movingToThiSide))
            motor.Play();
        else if (motor.isPlaying && !movingForward && !movingToThiSide)
            motor.Stop();
    }

    private void ResetPosition()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        RBody.velocity = Vector3.zero;
        RBody.angularVelocity = Vector3.zero;

        if (LeftMotor.isPlaying)
            LeftMotor.Stop();
        if (RigthMotor.isPlaying)
            RigthMotor.Stop();
    }

    #endregion Methods
}