using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Bullet BulletPrefab;
    public Rigidbody RBody;
    public int Points { get; private set; }
    public int Health { get; private set; }


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
        RBody.AddForce((transform.forward * v * Main.Instance.Settings.MoveSpeed) - RBody.velocity, ForceMode.Force);
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
        bullet.transform.position = transform.position + transform.forward * Main.Instance.Settings.BulletShottingDistance;
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