using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Bullet BulletPrefab;

    public Rigidbody RigidBody;
    public float MoveSpeed = 20f;
    public float RotateSpeed = 160f;

    private void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Vertical");
        RigidBody.AddForce((transform.forward * v * MoveSpeed) - RigidBody.velocity, ForceMode.Force);
    }

    private void Update()
    {
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
        bullet.transform.position = transform.position + transform.forward * 5;
    }
}