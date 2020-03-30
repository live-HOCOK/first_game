using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject bullet;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.01f));
        startPosition.z = 0f;
        transform.position = startPosition;

    }

    void Update()
    {
        Aiming();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }

    private void Aiming()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 moveDirection = mousePos - (Vector2)transform.position;

        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
