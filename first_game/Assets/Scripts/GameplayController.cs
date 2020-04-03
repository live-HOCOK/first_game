using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject brick;

    void Start()
    {
        SpawnBricks(4);
    }

    void Update()
    {
        
    }

    void SpawnBricks(int count)
    {
        List<Vector2> listPlaces = PlaceForSpawn();

        //randomize listPlaces
        System.Random rnd = new System.Random();
        for (int i = listPlaces.Count - 1; i >= 1; i--)
        {
            int j = rnd.Next(i + 1);
            var temp = listPlaces[j];
            listPlaces[j] = listPlaces[i];
            listPlaces[i] = temp;
        }

        for (int i = 0; i < count; i++)
        {
            Instantiate(brick, listPlaces[i], Quaternion.identity);
        }
        
    }

    List<Vector2> PlaceForSpawn()
    {
        List<Vector2> coordSpawnBricks = new List<Vector2>();

        //border for spawn
        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0.01f, 0f)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(0.99f, 0f)).x;
        float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.95f)).y;

        float widthBrick = brick.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

        int countPlace = (int)((Mathf.Abs(leftBorder) + rightBorder) / widthBrick) - 1;

        float nextCoord = leftBorder;
        float residue = (Mathf.Abs(leftBorder) + rightBorder) % widthBrick;

        for (int i = 1; i <= countPlace; i++)
        {
            if (residue == 0)
            {
                coordSpawnBricks.Add(new Vector2(nextCoord + widthBrick, topBorder));
                nextCoord += widthBrick;
            }
            else
            {
                coordSpawnBricks.Add(new Vector2(nextCoord + widthBrick + (residue / countPlace), topBorder));
                nextCoord += widthBrick + (residue / countPlace);
            }
        }

        return coordSpawnBricks;
    }
}
