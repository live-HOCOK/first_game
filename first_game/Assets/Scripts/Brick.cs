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
    static readonly Dictionary<int, Vector3> front = new Dictionary<int, Vector3>
    {
        [1] = new Vector3(-3.03f, 5.5f, -3.03f),
        [2] = new Vector3(-2.02f, 5.5f, -3.03f),
        [3] = new Vector3(-1.01f, 5.5f, -3.03f),
        [4] = new Vector3(0f, 5.5f, -3.03f),
        [5] = new Vector3(1.01f, 5.5f, -3.03f),
        [6] = new Vector3(2.02f, 5.5f, -3.03f)
    };
    static readonly Dictionary<int, Vector3> left = new Dictionary<int, Vector3>
    {
        [1] = new Vector3(-3.03f, 5.5f, 3.03f),
        [2] = new Vector3(-3.03f, 5.5f, 2.02f),
        [3] = new Vector3(-3.03f, 5.5f, 1.01f),
        [4] = new Vector3(-3.03f, 5.5f, 0f),
        [5] = new Vector3(-3.03f, 5.5f, -1.01f),
        [6] = new Vector3(-3.03f, 5.5f, -2.02f)
    };
    static readonly Dictionary<int, Vector3> back = new Dictionary<int, Vector3>
    {
        [1] = new Vector3(3.03f, 5.5f, 3.03f),
        [2] = new Vector3(2.02f, 5.5f, 3.03f),
        [3] = new Vector3(1.01f, 5.5f, 3.03f),
        [4] = new Vector3(0f, 5.5f, 3.03f),
        [5] = new Vector3(-1.01f, 5.5f, 3.03f),
        [6] = new Vector3(-2.02f, 5.5f, 3.03f)
    };
    static readonly Dictionary<int, Vector3> right = new Dictionary<int, Vector3>
    {
        [1] = new Vector3(3.03f, 5.5f, -3.03f),
        [2] = new Vector3(3.03f, 5.5f, -2.02f),
        [3] = new Vector3(3.03f, 5.5f, -1.01f),
        [4] = new Vector3(3.03f, 5.5f, 0f),
        [5] = new Vector3(3.03f, 5.5f, 1.01f),
        [6] = new Vector3(3.03f, 5.5f, 2.02f)
    };

    static public Vector3 GetPositionOnRightSwipe(Vector3 pos)
    {
        if (IsEqualsPosition(pos, front[1]))
            return left[1];
        else if (IsEqualsPosition(pos, front[2]))
            return left[2];
        else if (IsEqualsPosition(pos, front[3]))
            return left[3];
        else if (IsEqualsPosition(pos, front[4]))
            return left[4];
        else if (IsEqualsPosition(pos, front[5]))
            return left[5];
        else if (IsEqualsPosition(pos, front[6]))
            return left[6];
        else if (IsEqualsPosition(pos, left[1]))
            return back[1];
        else if (IsEqualsPosition(pos, left[2]))
            return back[2];
        else if (IsEqualsPosition(pos, left[3]))
            return back[3];
        else if (IsEqualsPosition(pos, left[4]))
            return back[4];
        else if (IsEqualsPosition(pos, left[5]))
            return back[5];
        else if (IsEqualsPosition(pos, left[6]))
            return back[6];
        else if (IsEqualsPosition(pos, back[1]))
            return right[1];
        else if (IsEqualsPosition(pos, back[2]))
            return right[2];
        else if (IsEqualsPosition(pos, back[3]))
            return right[3];
        else if (IsEqualsPosition(pos, back[4]))
            return right[4];
        else if (IsEqualsPosition(pos, back[5]))
            return right[5];
        else if (IsEqualsPosition(pos, back[6]))
            return right[6];
        else if (IsEqualsPosition(pos, right[1]))
            return front[1];
        else if (IsEqualsPosition(pos, right[2]))
            return front[2];
        else if (IsEqualsPosition(pos, right[3]))
            return front[3];
        else if (IsEqualsPosition(pos, right[4]))
            return front[4];
        else if (IsEqualsPosition(pos, right[5]))
            return front[5];
        else if (IsEqualsPosition(pos, right[6]))
            return front[6];
        return Vector3.zero;
    }

    static public Vector3 GetPositionOnLeftSwipe(Vector3 pos)
    {
        if (IsEqualsPosition(pos, front[1]))
            return right[1];
        else if (IsEqualsPosition(pos, front[2]))
            return right[2];
        else if (IsEqualsPosition(pos, front[3]))
            return right[3];
        else if (IsEqualsPosition(pos, front[4]))
            return right[4];
        else if (IsEqualsPosition(pos, front[5]))
            return right[5];
        else if (IsEqualsPosition(pos, front[6]))
            return right[6];
        else if (IsEqualsPosition(pos, right[1]))
            return back[1];
        else if (IsEqualsPosition(pos, right[2]))
            return back[2];
        else if (IsEqualsPosition(pos, right[3]))
            return back[3];
        else if (IsEqualsPosition(pos, right[4]))
            return back[4];
        else if (IsEqualsPosition(pos, right[5]))
            return back[5];
        else if (IsEqualsPosition(pos, right[6]))
            return back[6];
        else if (IsEqualsPosition(pos, back[1]))
            return left[1];
        else if (IsEqualsPosition(pos, back[2]))
            return left[2];
        else if (IsEqualsPosition(pos, back[3]))
            return left[3];
        else if (IsEqualsPosition(pos, back[4]))
            return left[4];
        else if (IsEqualsPosition(pos, back[5]))
            return left[5];
        else if (IsEqualsPosition(pos, back[6]))
            return left[6];
        else if (IsEqualsPosition(pos, left[1]))
            return front[1];
        else if (IsEqualsPosition(pos, left[2]))
            return front[2];
        else if (IsEqualsPosition(pos, left[3]))
            return front[3];
        else if (IsEqualsPosition(pos, left[4]))
            return front[4];
        else if (IsEqualsPosition(pos, left[5]))
            return front[5];
        else if (IsEqualsPosition(pos, left[6]))
            return front[6];
        return Vector3.zero;
    }

    static public bool IsEqualsPosition(Vector3 pos1, Vector3 pos2)
    {
        return (pos1.x == pos2.x && pos1.z == pos2.z);
    }

    static public Vector3 GetBrickCoord(int side, int position)
    {
        switch (side)
        {
            case Constants.FRONT_SIDE:
                return front[position];
            case Constants.LEFT_SIDE:
                return left[position];
            case Constants.BACK_SIDE:
                return back[position];
            case Constants.RIGHT_SIDE:
                return right[position];
            default:
                return Vector3.zero;
        }
    }
}
