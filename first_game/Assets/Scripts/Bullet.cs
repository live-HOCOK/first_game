using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.3f;

    private Rigidbody2D rb;
    private float right;
    private float left;
    private float top;
    private float bottom;
    private Vector2 force;
    private bool onScreen = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        right = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f)).x;
        left = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f)).x;
        top = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f)).y;
        bottom = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f)).y;
        force = transform.up * speed;
    }

    void FixedUpdate()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        rb.AddForce(force);
        if (transform.position.x >= right && onScreen)
            ReverseX();
        else if (transform.position.x <= left && onScreen)
            ReverseX();
        else if (transform.position.y >= top && onScreen)
            ReverseY();
        else if (transform.position.y <= bottom)
            DestroyBullet();
        else
            onScreen = true;
    }

    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    void ReverseX()
    {
        onScreen = false;
        force.x = -force.x;
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    void ReverseY()
    {
        onScreen = false;
        force.y = -force.y;
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
    }
}