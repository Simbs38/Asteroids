using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Bullet BulletPrefab;
    public int Health = 4;
    public Rigidbody RigidBody;
    public float MoveSpeed = 20f;
    public float RotateSpeed = 160f;

    private void FixedUpdate()
    {
        if (!Main.Instance.IsGameRunning)
            return;

        float v = Input.GetAxisRaw("Vertical");
        RigidBody.AddForce((transform.forward * v * MoveSpeed) - RigidBody.velocity, ForceMode.Force);
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
    }
}