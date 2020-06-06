using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private Vector2 force;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetForce();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("CollisionBorder"))
            DestroyBullet();
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.velocity = new Vector2((Mathf.Sign(rb.velocity.x) == Mathf.Sign(force.x)) ? force.x : -force.x,
            (Mathf.Sign(rb.velocity.y) == Mathf.Sign(force.y)) ? force.y : -force.y);
    }

    private void DestroyBullet()
    {
        PoolBalls.Inactivate(gameObject);
    }

    private void SetForce()
    {
        force = transform.up * speed;
        rb.velocity = force;
    }

    public void NewStart(Vector3 pos, Quaternion rotation)
    {
        transform.position = pos;
        transform.rotation = rotation;
        SetForce();
    }
}