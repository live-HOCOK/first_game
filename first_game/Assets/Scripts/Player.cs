using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;
    public int countBalls = 1;
    public float cooldown = 0.5f;

    private Vector2 startPosition;
    private float nextShoot = 0f;
    private int countBallsOnShoot;
    private bool readyShoot = true;
    private bool canAim = true;
    private GameObject cameraObj;
    private GameObject arrow;

    void Start()
    {
        startPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0f));
        transform.position = startPosition;
        countBallsOnShoot = countBalls;

        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        arrow = transform.Find("Arrow").gameObject;

        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
    }

    void Update()
    {
        if (canAim && Input.GetButton("Fire1"))
            Aiming();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetButtonUp("Fire1") && readyShoot)
        {
            canAim = false;
            readyShoot = false;
            arrow.SetActive(false);
            InstantiateBall();
        }
        else if (nextShoot < Time.time && countBallsOnShoot > 0 && !readyShoot)
            InstantiateBall();
    }

    private void InstantiateBall()
    {
        nextShoot = Time.time + cooldown;
        countBallsOnShoot--;
        if (PoolBalls.CountInactive() == 0)
        {
            GameObject ball = Instantiate(bullet, transform.position, transform.rotation);
            ball.GetComponent<Bullet>().SetGameplayController(cameraObj);
            PoolBalls.Add(ball);
        }
        else
        {
            GameObject ball = PoolBalls.ActiveNext(startPosition, transform.rotation);
        }
    }

    private void Aiming()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPosition;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos);
        if (hit.collider != null)
        {
            arrow.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            arrow.GetComponent<LineRenderer>().SetPosition(1, hit.point);
            arrow.SetActive(true);
            
        }

        if (mousePos != Vector2.zero)
        {
            float angle = Mathf.Atan2(-mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnDestroyAllBalls()
    {
        countBalls++;
        countBallsOnShoot = countBalls;
        readyShoot = true;
        canAim = true;
        startPosition = new Vector2(PoolBalls.NextPosition(), startPosition.y);
        transform.position = startPosition;
    }
}
