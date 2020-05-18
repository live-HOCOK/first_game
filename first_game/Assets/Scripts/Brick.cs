using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private int hp = 1;

    private float bottomScreen;
    private GameplayController gameController;
    private Vector3 newPosition;
    private bool move = false;

    void Start()
    {
        gameController = Camera.main.GetComponent<GameplayController>();
        bottomScreen = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.01f, gameController.GetGameClipPlane())).y;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hp--;
            if (hp <= 0)
                Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        float heightBrick = GetComponent<MeshRenderer>().bounds.extents.y * 2;
        transform.position -= new Vector3(0, heightBrick, 0);
        if (transform.position.y <= bottomScreen)
        {
            GameEvents.onBrickTouchingBottom.Invoke();
            Destroy(gameObject);
        }
    }

    public void SetHP(int newHP) { hp = newHP; }

    public int GetHP() { return hp; }

    private void OnDestroyAllBalls()
    {
        MoveDown();
    }
    
    private void OnSwipeRight()
    {
        if (!move)
        {
            move = true;
            newPosition = BrickPosition.GetPositionOnRightSwipe(transform.position);
        }
    }

    private void OnSwipeLeft()
    {
        if (!move)
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

public static class BrickPosition
{
    static Vector3 frontFirst = new Vector3(-3.03f, 0f, -3.03f);
    static Vector3 frontSecond = new Vector3(-2.02f, 0f, -3.03f);
    static Vector3 frontThird = new Vector3(-1.01f, 0f, -3.03f);
    static Vector3 frontFourth = new Vector3(0f, 0f, -3.03f);
    static Vector3 frontFifth = new Vector3(1.01f, 0f, -3.03f);
    static Vector3 frontSixth = new Vector3(2.02f, 0f, -3.03f);
    static Vector3 leftFirst = new Vector3(-3.03f, 0f, 3.03f);
    static Vector3 leftSecond = new Vector3(-3.03f, 0f, 2.02f);
    static Vector3 leftThird = new Vector3(-3.03f, 0f, 1.01f);
    static Vector3 leftFourth = new Vector3(-3.03f, 0f, 0f);
    static Vector3 leftFifth = new Vector3(-3.03f, 0f, -1.01f);
    static Vector3 leftSixth = new Vector3(-3.03f, 0f, -2.02f);
    static Vector3 backFirst = new Vector3(3.03f, 0f, 3.03f);
    static Vector3 backSecond = new Vector3(2.02f, 0f, 3.03f);
    static Vector3 backThird = new Vector3(1.01f, 0f, 3.03f);
    static Vector3 backFourth = new Vector3(0f, 0f, 3.03f);
    static Vector3 backFifth = new Vector3(-1.01f, 0f, 3.03f);
    static Vector3 backSixth = new Vector3(-2.02f, 0f, 3.03f);
    static Vector3 rightFirst = new Vector3(3.03f, 0f, -3.03f);
    static Vector3 rightSecond = new Vector3(3.03f, 0f, -2.02f);
    static Vector3 rightThird = new Vector3(3.03f, 0f, -1.01f);
    static Vector3 rightFourth = new Vector3(3.03f, 0f, 0f);
    static Vector3 rightFifth = new Vector3(3.03f, 0f, 1.01f);
    static Vector3 rightSixth = new Vector3(3.03f, 0f, 2.02f);

    static public Vector3 GetPositionOnRightSwipe(Vector3 pos)
    {
        if (IsEqualsPosition(pos, frontFirst))
            return leftFirst;
        else if (IsEqualsPosition(pos, frontSecond))
            return leftSecond;
        else if (IsEqualsPosition(pos, frontThird))
            return leftThird;
        else if (IsEqualsPosition(pos, frontFourth))
            return leftFourth;
        else if (IsEqualsPosition(pos, frontFifth))
            return leftFifth;
        else if (IsEqualsPosition(pos, frontSixth))
            return leftSixth;
        else if (IsEqualsPosition(pos, leftFirst))
            return backFirst;
        else if (IsEqualsPosition(pos, leftSecond))
            return backSecond;
        else if (IsEqualsPosition(pos, leftThird))
            return backThird;
        else if (IsEqualsPosition(pos, leftFourth))
            return backFourth;
        else if (IsEqualsPosition(pos, leftFifth))
            return backFifth;
        else if (IsEqualsPosition(pos, leftSixth))
            return backSixth;
        else if (IsEqualsPosition(pos, backFirst))
            return rightFirst;
        else if (IsEqualsPosition(pos, backSecond))
            return rightSecond;
        else if (IsEqualsPosition(pos, backThird))
            return rightThird;
        else if (IsEqualsPosition(pos, backFourth))
            return rightFourth;
        else if (IsEqualsPosition(pos, backFifth))
            return rightFifth;
        else if (IsEqualsPosition(pos, backSixth))
            return rightSixth;
        else if (IsEqualsPosition(pos, rightFirst))
            return frontFirst;
        else if (IsEqualsPosition(pos, rightSecond))
            return frontSecond;
        else if (IsEqualsPosition(pos, rightThird))
            return frontThird;
        else if (IsEqualsPosition(pos, rightFourth))
            return frontFourth;
        else if (IsEqualsPosition(pos, rightFifth))
            return frontFifth;
        else if (IsEqualsPosition(pos, rightSixth))
            return frontSixth;
        return Vector3.zero;
    }

    static public Vector3 GetPositionOnLeftSwipe(Vector3 pos)
    {
        if (IsEqualsPosition(pos, frontFirst))
            return rightFirst;
        else if (IsEqualsPosition(pos, frontSecond))
            return rightSecond;
        else if (IsEqualsPosition(pos, frontThird))
            return rightThird;
        else if (IsEqualsPosition(pos, frontFourth))
            return rightFourth;
        else if (IsEqualsPosition(pos, frontFifth))
            return rightFifth;
        else if (IsEqualsPosition(pos, frontSixth))
            return rightSixth;
        else if (IsEqualsPosition(pos, rightFirst))
            return backFirst;
        else if (IsEqualsPosition(pos, rightSecond))
            return backSecond;
        else if (IsEqualsPosition(pos, rightThird))
            return backThird;
        else if (IsEqualsPosition(pos, rightFourth))
            return backFourth;
        else if (IsEqualsPosition(pos, rightFifth))
            return backFifth;
        else if (IsEqualsPosition(pos, rightSixth))
            return backSixth;
        else if (IsEqualsPosition(pos, backFirst))
            return leftFirst;
        else if (IsEqualsPosition(pos, backSecond))
            return leftSecond;
        else if (IsEqualsPosition(pos, backThird))
            return leftThird;
        else if (IsEqualsPosition(pos, backFourth))
            return leftFourth;
        else if (IsEqualsPosition(pos, backFifth))
            return leftFifth;
        else if (IsEqualsPosition(pos, backSixth))
            return leftSixth;
        else if (IsEqualsPosition(pos, leftFirst))
            return frontFirst;
        else if (IsEqualsPosition(pos, leftSecond))
            return frontSecond;
        else if (IsEqualsPosition(pos, leftThird))
            return frontThird;
        else if (IsEqualsPosition(pos, leftFourth))
            return frontFourth;
        else if (IsEqualsPosition(pos, leftFifth))
            return frontFifth;
        else if (IsEqualsPosition(pos, leftSixth))
            return frontSixth;
        return Vector3.zero;
    }

    static public bool IsEqualsPosition(Vector3 pos1, Vector3 pos2)
    {
        return (pos1.x == pos2.x && pos1.z == pos2.z);
    }
}
