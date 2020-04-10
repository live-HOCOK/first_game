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
    private bool onScreenY = true;
    private bool onScreenX = true;

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
        Vector2 brickCoord = hitObject.transform.position;

        //coord borders of collision
        float widthBrick = hitObject.GetComponent<SpriteRenderer>().bounds.extents.x;
        float heightBrick = hitObject.GetComponent<SpriteRenderer>().bounds.extents.y;
        Vector2 topLeftBrick = new Vector2(brickCoord.x - widthBrick, brickCoord.y + heightBrick);
        Vector2 topRightBrick = new Vector2(brickCoord.x + widthBrick, brickCoord.y + heightBrick);
        Vector2 bottomLeftBrick = new Vector2(brickCoord.x - widthBrick, brickCoord.y - heightBrick);
        Vector2 bottomRightBrick = new Vector2(brickCoord.x + widthBrick, brickCoord.y - heightBrick);

        if (transform.position.x > bottomLeftBrick.x && transform.position.x < bottomRightBrick.x)
            ReverseY();
        else if (transform.position.y > bottomLeftBrick.y && transform.position.y < topLeftBrick.y)
            ReverseX();
    }

    private void MoveBullet()
    {
        rb.velocity = force;
        if ((transform.position.x >= right || transform.position.x <= left) && onScreenX)
            ReverseX();
        else if (transform.position.y >= top && onScreenY)
            ReverseY();
        else if (transform.position.y <= bottom)
            DestroyBullet();
        if (transform.position.x < right && transform.position.x > left)
            onScreenX = true;
        if (transform.position.y < top)
            onScreenY = true;
    }

    void DestroyBullet()
    {
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 1)
            GameEvents.onDestroyAllBalls.Invoke();
        Destroy(gameObject);
    }

    void ReverseX()
    {
        onScreenX = false;
        force.x = -force.x;
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    void ReverseY()
    {
        onScreenY = false;
        force.y = -force.y;
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
    }
}