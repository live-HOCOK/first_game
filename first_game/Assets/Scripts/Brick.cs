using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private int hp = 1;

    private TextMesh textCount;
    
    void Start()
    {
        textCount = GetComponentInChildren<TextMesh>();
        textCount.text = hp.ToString();
        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
    }

    public void setHP(int newHP)
    {
        hp = newHP;
        Debug.Log(hp);
    }
}
