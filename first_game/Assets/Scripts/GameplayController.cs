using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject borders;
    public GameObject brick;

    private float gameClipPlane;

    private void Awake()
    {
        //if (SaveLoad.CheckSave())
        //    countBalls = GameState.GetCountBalls();
        //else
        //    countBalls = 1;
        gameClipPlane = Camera.main.nearClipPlane + 1f;
    }

    void Start()
    {
        SetBorders();

        GameEvents.onDestroyAllBalls.AddListener(onDestroyAllBalls);
    }

    private void SetBorders()
    {
        borders.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, gameClipPlane));
    }

    private void SpawnNewVawe(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            Vector3 newBrickPosition = BrickPosition.GetBrickCoord(Random.Range(0, 4), Random.Range(1, 6));
            GameObject newBrick = Instantiate(brick, newBrickPosition, Quaternion.identity);
            newBrick.GetComponent<Brick>().SetHP(1);
        }
    }

    private void onDestroyAllBalls()
    {
        SpawnNewVawe(8);
    }

    public float GetGameClipPlane() { return gameClipPlane; }

}
