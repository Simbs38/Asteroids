using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Bullet BulletPrefab;
    public int StartingHealt = 4;
    public int Health;
    public Rigidbody RBody;
    public float MoveSpeed = 20f;
    public float RotateSpeed = 160f;
    public int Points { get; private set; }

    private void Start()
    {
        Health = StartingHealt;
        Points = 0;
    }

    private void FixedUpdate()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float v = Input.GetAxisRaw("Vertical");
        RBody.AddForce((transform.forward * v * MoveSpeed) - RBody.velocity, ForceMode.Force);
    }

    private void Update()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float rotation = Input.GetAxisRaw("Horizontal");
        Vector3 currentEulerRotation = transform.eulerAngles;
        currentEulerRotation.y += rotation * Time.smoothDeltaTime * RotateSpeed;
        transform.localEulerAngles = currentEulerRotation;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    public void UpdateScore()
    {
        Points++;
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(BulletPrefab);
        bullet.Direction = transform.forward;
        bullet.transform.position = transform.position + transform.forward * 3;
    }

    public void TakeHit()
    {
        Health--;
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        RBody.velocity = Vector3.zero;
        RBody.angularVelocity = Vector3.zero;
    }
}