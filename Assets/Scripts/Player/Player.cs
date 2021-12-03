using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour, IDamageable
{
    #region Fields

    public static Player Instance;

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

    #endregion Fields

    #region Unity Methods

    private void Awake() => Instance = this;

    private void Start()
    {
        Health = GameStateManager.Instance.Settings.Player.StartingHealt;
        Points = 0;
        MeshR.material.color = GameStateManager.Instance.Settings.Player.PlayerColor;
        HealthUIManager.Instance.PopulateHealtUI(GameStateManager.Instance.Settings.Player.StartingHealt);
    }

    private void FixedUpdate()
    {
        if (!GameStateManager.Instance.IsGameRunning)
            return;

        float movement = GetMovement();
        float rotation = GetRotation();

        SetMotor(LeftMotor, 0 < movement, Mathf.Sign(rotation) == 1 && rotation != 0);
        SetMotor(RigthMotor, 0 < movement, Mathf.Sign(rotation) == -1 && rotation != 0);

        RBody.AddForce((transform.forward * movement * GameStateManager.Instance.Settings.Player.MoveSpeed) - RBody.velocity, ForceMode.Force);
    }

    private void Update()
    {
        if (!GameStateManager.Instance.IsGameRunning)
            return;

        float rotation = GetRotation();
        Vector3 currentEulerRotation = transform.eulerAngles;
        currentEulerRotation.y += rotation * Time.smoothDeltaTime * GameStateManager.Instance.Settings.Player.RotateSpeed;
        transform.localEulerAngles = currentEulerRotation;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    #endregion Unity Methods

    #region Methods

    public void Shoot()
    {
        Bullet bullet = Instantiate(BulletPrefab);
        Vector3 spwanPosition = transform.position + transform.forward * GameStateManager.Instance.Settings.Bullet.ShottingDistance;
        bullet.Shooting(spwanPosition, transform.forward);
    }

    public void AddScore() => Points += GameStateManager.Instance.Settings.Asteroid.HitPoints;

    public void TakeDamage()
    {
        Sound.Play();
        Health--;
        ResetPosition();
        HealthUIManager.Instance.RemoveHealt();

        if (Health == 0)
            GameStateManager.Instance.EndGame(Points);
        else
            GameStateManager.Instance.PauseGame();
    }

    public bool CreateExplosion() => false;

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