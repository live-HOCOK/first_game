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
        if (Input.GetButton("Fire1"))
            Aiming();
        else if (arrow.enabled == true)
            arrow.enabled = false;
    }

    private void Aiming()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, gameController.GetGameClipPlane())) - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            if (arrow.enabled == false)
                arrow.enabled = true;
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, hit.point);
        }

        if (mousePos != Vector3.zero)
        {
            float angle = Mathf.Atan2(-mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void InstantiateBall()
    {
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

    private void OnDestroyAllBalls()
    {

    }

    private void OnTap()
    {
        if ((canShoot) || (!canShoot && countBallsOnCurrentShoot > 0 && timeNextShoot < Time.time))
            InstantiateBall();
    }
}
