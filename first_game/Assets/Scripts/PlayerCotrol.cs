using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotrol : MonoBehaviour
{
    public GameObject ball;
    public float coolDown = 0.3f;

    private int countBalls;
    private GameplayController gameController;
    private LineRenderer arrow;
    private bool canShoot = true;
    private float timeNextShoot;
    private int countBallsOnCurrentShoot;

    void Start()
    {
        arrow = GetComponent<LineRenderer>();
        gameController = Camera.main.GetComponent<GameplayController>();

        if (SaveLoad.CheckSave())
        {
            transform.position = GameState.GetSavedPosition().ToVector3();
            countBalls = GameState.GetCountBalls();
        }
        else
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, gameController.GetGameClipPlane()));
            countBalls = 1;
        }

        countBallsOnCurrentShoot = countBalls;

        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
        SwipeManager.OnSingleTap += OnTap;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.timeScale > 0 && canShoot)
            Aiming();
        else if (arrow.enabled == true)
            arrow.enabled = false;
        if (!canShoot && countBallsOnCurrentShoot > 0 && timeNextShoot < Time.time)
            InstantiateBall();
    }

    private void Aiming()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, gameController.GetGameClipPlane())) - transform.position;

        if (mousePos != Vector3.zero)
        {
            float angle = Mathf.Atan2(-mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, hit.point);
            if (arrow.enabled == false)
                arrow.enabled = true;
        }
        
    }

    private void InstantiateBall()
    {
        canShoot = false;
        timeNextShoot = Time.time + coolDown;
        countBallsOnCurrentShoot--;
        if (PoolBalls.CountInactive() == 0)
        {
            GameObject newBall = Instantiate(ball, transform.position, transform.rotation);
            PoolBalls.Add(newBall);
        }
        else
        {
            PoolBalls.ActiveNext(transform.position, transform.rotation);
        }
    }

    public int GetCountBalls() { return countBalls; }

    public bool GetCanShoot() { return canShoot; }

    private void OnDestroyAllBalls()
    {
        transform.position = new Vector3(PoolBalls.NextPosition(), transform.position.y, transform.position.z);
        countBalls++;
        countBallsOnCurrentShoot = countBalls;
        canShoot = true;
    }

    private void OnTap()
    {
        if (canShoot)
            InstantiateBall();
    }
}
