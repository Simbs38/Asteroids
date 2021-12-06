using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour, IDamageable
{
    #region Fields

    public int Points { get; private set; }
    public int Health { get; private set; }

    [SerializeField]
    private Bullet BulletPrefab;

    [SerializeField]
    private Rigidbody RBody;

    [SerializeField]
    private ParticleSystem LeftMotor;

    [SerializeField]
    private ParticleSystem RigthMotor;

    [SerializeField]
    private Joystick CurrentJoystick;

    [SerializeField]
    private AudioSource Sound;

    [SerializeField]
    private MeshRenderer MeshR;

    [SerializeField]
    private Text ScoreUI;

    [SerializeField]
    private PlayerSettings Settings;

    [SerializeField]
    private GameStateManager Manager;

    [SerializeField]
    private HealthUIManager HealthUI;
    private Queue<Bullet> _bulletPool;

    #endregion Fields

    #region Unity Methods

    private void Start()
    {
        Health = Settings.StartingHealt;
        Points = 0;
        MeshR.material.color = Settings.PlayerColor;
        HealthUI.PopulateHealtUI(Settings.StartingHealt);
        _bulletPool = new Queue<Bullet>();

        for (int i = 0; i < Settings.BulletPoolSize; i++)
        {
            Bullet tmp = Instantiate(BulletPrefab);
            _bulletPool.Enqueue(tmp);
            tmp.Init(this);
        }
    }

    private void FixedUpdate()
    {
        if (!Manager.IsGameRunning)
            return;

        float movement = GetMovement();
        float rotation = GetRotation();

        SetMotor(LeftMotor, 0 < movement, Mathf.Sign(rotation) == 1 && rotation != 0);
        SetMotor(RigthMotor, 0 < movement, Mathf.Sign(rotation) == -1 && rotation != 0);

        RBody.AddForce((transform.forward * movement * Settings.MoveSpeed) - RBody.velocity, ForceMode.Force);
    }

    private void Update()
    {
        if (!Manager.IsGameRunning)
            return;

        float rotation = GetRotation();
        Vector3 currentEulerRotation = transform.eulerAngles;
        currentEulerRotation.y += rotation * Time.smoothDeltaTime * Settings.RotateSpeed;
        transform.localEulerAngles = currentEulerRotation;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    #endregion Unity Methods

    #region Methods

    public void Shoot()
    {
        if(_bulletPool.Count == 0)
            return;

        Bullet bullet = _bulletPool.Dequeue();
        Vector3 spwanPosition = transform.position + transform.forward * bullet.ShootingDistance;
        bullet.Shooting(spwanPosition, transform.forward);
    }

    public void RecicleBullet(Bullet bullet) => _bulletPool.Enqueue(bullet);

    public void AddScore(int points)
    {
        Points += points;
        ScoreUI.text = Points.ToString();
    }

    public void TakeDamage()
    {
        Sound.Play();
        Health--;
        ResetPosition();
        HealthUI.RemoveHealt();

        if (Health == 0)
            Manager.EndGame(Points);
        else
            Manager.PauseGame();
    }

    public bool IsSucessfullHit(out int hitPoints)
    {
        hitPoints = 0;
        return false;
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