using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject wood_brick;
    public GameObject stone_brick;
    public GameObject leftBorder;
    public GameObject rightBorder;
    public GameObject topBorder;
    public GameObject bottomBorder;

    private float gameClipPlane;
    private int numberlevel = 0;
    private int currentWave = 0;
    private GameTools.level currentLevel;

    private void Awake()
    {
        gameClipPlane = Camera.main.nearClipPlane + 2f;
    }

    void Start()
    {
        SetBorders();

        currentLevel = Level.GetLevel(numberlevel);
        SpawnBricks();

        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
        GameEvents.onClickLoosingMessage.AddListener(OnClickLoosingMessage);
        GameEvents.onBrickTouchingBottom.AddListener(OnBrickTouchingBottom);
    }

    private void SetBorders()
    {
        leftBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, gameClipPlane));
        rightBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, gameClipPlane));
        topBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, gameClipPlane));
        bottomBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.05f, gameClipPlane));
    }

    private void SpawnBricks()
    {
        foreach (GameTools.brickInf brick in currentLevel.GetPoolBricks())
        {
            if (brick.GetWave() == currentWave)
            {
                Vector3 newBrickPosition = BrickPosition.GetBrickCoord(brick.GetSide(), brick.GetPosition());
                GameObject goBrick = GetBrickOnType(brick.GetBrickType());
                Instantiate(goBrick, newBrickPosition, Quaternion.identity);
            }
        }
    }

    private GameObject GetBrickOnType(int type)
    {
        switch (type)
        {
            case GameTools.BRICK_WOOD:
                return wood_brick;
            case GameTools.BRICK_STONE:
                return stone_brick;
            default:
                return new GameObject();
        }
    }

    private void OnDestroyAllBalls()
    {
        currentWave++;
        SpawnBricks();
    }

    private void OnClickLoosingMessage()
    {
        Time.timeScale = 1;
    }

    private void OnBrickTouchingBottom()
    {
        Time.timeScale = 0;
    }

    public float GetGameClipPlane() { return gameClipPlane; }

    public void SetLevel(int level) { this.numberlevel = level; }

}
