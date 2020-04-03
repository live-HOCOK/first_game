using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int count = 1;

    private TextMesh textCount;
    
    void Start()
    {
        textCount = GetComponentInChildren<TextMesh>();
        textCount.text = count.ToString();
    }

    void FixedUpdate()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            count--;
            if (count == 0)
                Destroy(gameObject);
            else
                textCount.text = count.ToString();
        }
    }
}
