using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject borders;

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
    }

    private void SetBorders()
    {
        borders.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, gameClipPlane));
    }

    public float GetGameClipPlane() { return gameClipPlane; }

}
