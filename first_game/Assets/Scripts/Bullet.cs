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
        bottom = Camera.main.ViewportToWorldPoint(new Vector3(0f, -0.01f)).y;
        force = transform.up * speed;
    }

    void FixedUpdate()
    {
        MoveBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;
        if (hitObject.CompareTag("Brick")) {
            Vector2 brickCoord = hitObject.transform.position;

            //coord of border point
            float width = hitObject.GetComponent<SpriteRenderer>().bounds.extents.x;
            float height = hitObject.GetComponent<SpriteRenderer>().bounds.extents.y;
            Vector2 topLeft = new Vector2(brickCoord.x - width, brickCoord.y + height);
            Vector2 topRight = new Vector2(brickCoord.x + width, brickCoord.y + height);
            Vector2 bottomLeft = new Vector2(brickCoord.x - width, brickCoord.y - height);
            Vector2 bottomRight = new Vector2(brickCoord.x + width, brickCoord.y - height);

            if (transform.position.x > bottomLeft.x && transform.position.x < bottomRight.x)
                ReverseY();
            else if (transform.position.y > bottomLeft.y && transform.position.y < topLeft.y)
                ReverseX();
        }
    }

    void MoveBullet()
    {
        rb.velocity = force;
        if ((transform.position.x >= right || transform.position.x <= left) && onScreen)
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
        Destroy(gameObject);
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