using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.3f;

    private GameObject cameraObject;
    private Rigidbody2D rb;
    private Vector2 force;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        force = transform.up * speed;
        rb.velocity = force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CollisionBorder"))
            DestroyBullet();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = new Vector2((Mathf.Sign(rb.velocity.x) == Mathf.Sign(force.x)) ? force.x : -force.x,
            (Mathf.Sign(rb.velocity.y) == Mathf.Sign(force.y)) ? force.y : -force.y);
    }

    private void DestroyBullet()
    {
        PoolBalls.Inactivate(gameObject);
    }

    public void SetGameplayController(GameObject obj)
    {
        cameraObject = obj;
    }

    public void NewStart(Vector2 pos)
    {
        
        transform.position = pos;
        //transform.rotation = Quaternion.identity;
        rb.velocity = force;
        Debug.Log(force);
    }
}