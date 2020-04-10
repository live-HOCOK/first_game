using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private int hp = 1;

    private TextMesh textCount;
    private float bottomScreen;
    
    void Start()
    {
        textCount = GetComponentInChildren<TextMesh>();
        textCount.text = hp.ToString();
        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
        bottomScreen = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.01f)).y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hp--;
            if (hp == 0)
                Destroy(gameObject);
            else
                textCount.text = hp.ToString();
        }
    }

    private void OnDestroyAllBalls()
    {
        MoveDown();       
    }

    private void MoveDown()
    {
        float heightBrick = GetComponent<SpriteRenderer>().bounds.extents.y * 2;
        transform.position -= new Vector3(0, heightBrick, 0);
        if (transform.position.y <= bottomScreen)
        {
            GameEvents.onBrickTouchingBottom.Invoke();
            Destroy(gameObject);
        }
    }

    public void setHP(int newHP)
    {
        hp = newHP;
    }
}
