using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction;
    private float BulletSpeed = 0.5f;

    private void Update() => transform.position += Direction * BulletSpeed;

    private void OnTriggerEnter() => Destroy(gameObject);
}