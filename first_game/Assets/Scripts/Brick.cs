using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private int hp = 1;
    private int maxHP = 1;

    private float bottomScreen;
    private GameplayController gameController;
    private Vector3 newPosition;
    private bool move = false;
    private float heightBrick;
    private GameObject player;

    void Start()
    {
        gameController = Camera.main.GetComponent<GameplayController>();
        bottomScreen = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.01f, gameController.GetGameClipPlane())).y;
        heightBrick = GetComponent<MeshRenderer>().bounds.extents.y * 2;
        player = GameObject.FindGameObjectWithTag("Player");

        transform.rotation = Quaternion.Euler(0, GetRandomAngle(), 0);

        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
        SwipeEvents.OnSwipeRight += OnSwipeRight;
        SwipeEvents.OnSwipeLeft += OnSwipeLeft;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(newPosition.x, transform.position.y, newPosition.z), 0.1f);
            if (BrickPosition.IsEqualsPosition(newPosition, transform.position))
                move = false;
        }
    }

    private int GetRandomAngle()
    {
        int rnd = Random.Range(0, 3);

        int result = 0;

        switch (rnd)
        {
            case 0:
                result = 180;
                return result;
            case 1:
                result = 90;
                return result;
            case 2:
                result = -90;
                return result;
            default:
                return result;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hp--;
            ChangeColor();
            if (hp <= 0)
                Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.position -= new Vector3(0, heightBrick, 0);
        if (transform.position.y <= bottomScreen)
        {
            GameEvents.onBrickTouchingBottom.Invoke();
            Destroy(gameObject);
        }
    }

    private void ChangeColor()
    {
        if (hp < maxHP && hp > 0)
        {
            float percentHP = (float)hp / (float)maxHP;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0 + percentHP, 0 + percentHP, 1);
        }
    }

    public void SetHP(int newHP, int newMaxHP)
    {
        hp = newHP;
        maxHP = newMaxHP;
        ChangeColor();
    }

    public int GetHP() { return hp; }

    public int GetMaxHP() { return maxHP; }

    private void OnDestroyAllBalls()
    {
        MoveDown();
    }
    
    private void OnSwipeRight()
    {
        if (!move && player.GetComponent<PlayerCotrol>().GetCanShoot())
        {
            move = true;
            newPosition = BrickPosition.GetPositionOnRightSwipe(transform.position);
        }
    }

    private void OnSwipeLeft()
    {
        if (!move && player.GetComponent<PlayerCotrol>().GetCanShoot())
        {
            move = true;
            newPosition = BrickPosition.GetPositionOnLeftSwipe(transform.position);
        }
    }

    private void OnDestroy()
    {
        SwipeEvents.OnSwipeRight -= OnSwipeRight;
        SwipeEvents.OnSwipeLeft -= OnSwipeLeft;
    }
}
