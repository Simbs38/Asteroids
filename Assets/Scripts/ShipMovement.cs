using UnityEngine;

public class ShipMovement : MonoBehaviour
{
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
    }
}