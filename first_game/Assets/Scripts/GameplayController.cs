using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject brick;
    public GameObject borders;

    private int hpBricks = 1;
    private float leftBorderSpawn;
    private float rightBorderSpawn;
    private float topBorderSpawn;
    private float widthBrick;
    private int countPlaceForSpawn;

    void Start()
    {
        SetBorders(); //position for collision borders

        //set border for spawn area
        leftBorderSpawn = Camera.main.ViewportToWorldPoint(new Vector3(0.01f, 0f)).x;
        rightBorderSpawn = Camera.main.ViewportToWorldPoint(new Vector3(0.99f, 0f)).x;
        topBorderSpawn = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.95f)).y;

        widthBrick = brick.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

        countPlaceForSpawn = (int)((Mathf.Abs(leftBorderSpawn) + rightBorderSpawn) / widthBrick) - 1;

        SpawnBricks((int)Random.Range(1,6));
        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
    }

    private void SpawnBricks(int count)
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
            GameObject newBrick = Instantiate(brick, listPlaces[i], Quaternion.identity);
            newBrick.GetComponent<Brick>().setHP(hpBricks);
        }

        hpBricks++;        
    }

    private List<Vector2> PlaceForSpawn()
    {
        List<Vector2> coordSpawnBricks = new List<Vector2>();

        float nextCoord = leftBorderSpawn;
        float residue = (Mathf.Abs(leftBorderSpawn) + rightBorderSpawn) % widthBrick;

        for (int i = 1; i <= countPlaceForSpawn; i++)
        {
            if (residue == 0)
            {
                coordSpawnBricks.Add(new Vector2(nextCoord + widthBrick, topBorderSpawn));
                nextCoord += widthBrick;
            }
            else
            {
                coordSpawnBricks.Add(new Vector2(nextCoord + widthBrick + (residue / countPlaceForSpawn), topBorderSpawn));
                nextCoord += widthBrick + (residue / countPlaceForSpawn);
            }
        }

        return coordSpawnBricks;
    }

    private void OnDestroyAllBalls()
    {
        SpawnBricks((int)Random.Range(1, 6));
    }

    private void SetBorders()
    {
        borders.transform.Find("leftBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f));
        borders.transform.Find("rightBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f));
        borders.transform.Find("topBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f));
        borders.transform.Find("bottomBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.03f));
    }
}
