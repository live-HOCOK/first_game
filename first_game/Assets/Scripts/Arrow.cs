﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject bullet;
    public int countBalls = 1;
    public float cooldown = 0.5f;

    private Vector3 startPosition;
    private float nextShoot = 0f;
    private int countBallsOnShoot;
    private bool readyShoot = true;
    private bool canAim = true;

    void Start()
    {
        startPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f));
        startPosition.z = 0f;
        transform.position = startPosition;
        countBallsOnShoot = countBalls;
        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
        GameEvents.onStartShooting.AddListener(OnStartShooting);
        GameEvents.onStopShooting.AddListener(OnStopShooting);
    }

    void Update()
    {
        if (canAim)
            Aiming();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && readyShoot)
        {
            GameEvents.onStartShooting.Invoke();
            readyShoot = false;
            InstantiateBall();
        }
        else if (nextShoot < Time.time && countBallsOnShoot > 0 && !readyShoot) {
            InstantiateBall();
        }
    }

    private void InstantiateBall()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        nextShoot = Time.time + cooldown;
        countBallsOnShoot--;
        if (countBallsOnShoot <= 0)
            GameEvents.onStopShooting.Invoke();
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

    private void OnDestroyAllBalls()
    {
        countBalls++;
        countBallsOnShoot = countBalls;
        readyShoot = true;
    }

    private void OnStartShooting()
    {
        canAim = false;
    }

    private void OnStopShooting()
    {
        canAim = true;
    }
}
