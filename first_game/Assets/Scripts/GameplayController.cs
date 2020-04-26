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
        borders.transform.Find("leftBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, gameClipPlane));
        borders.transform.Find("rightBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, gameClipPlane));
        borders.transform.Find("topBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, gameClipPlane));
        borders.transform.Find("bottomBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.03f, gameClipPlane));
    }

    public float GetGameClipPlane() { return gameClipPlane; }

}
