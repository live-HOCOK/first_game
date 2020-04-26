using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private int hp = 1;

    private float bottomScreen;
    private GameplayController gameController;
    
    void Start()
    {
        gameController = Camera.main.GetComponent<GameplayController>();
        bottomScreen = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.01f, gameController.GetGameClipPlane())).y;
        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
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

    public void SetHP(int newHP)
    {
        hp = newHP;
    }

    public int GetHP()
    {
        return hp;
    }

    private void OnDestroyAllBalls()
    {
        MoveDown();
    }
}
