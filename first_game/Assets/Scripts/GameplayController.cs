using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject borders;
    public GameObject brick;
    public int CountNewBricks = 8;

    private float gameClipPlane;
    private int hpBricks = 1;

    private void Awake()
    {
        gameClipPlane = Camera.main.nearClipPlane + 1f;
        SaveLoad.LoadGame();
    }

    void Start()
    {
        SetBorders();

        if (SaveLoad.CheckSave())
        {
            LoadBricks();
            hpBricks = GameState.GetHpNewBricks();
        }
        else
        {
            SpawnNewVawe(CountNewBricks);
        }

        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
        GameEvents.onClickLoosingMessage.AddListener(OnClickLoosingMessage);
        GameEvents.onBrickTouchingBottom.AddListener(OnBrickTouchingBottom);
    }

    private void SetBorders()
    {
        borders.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, gameClipPlane));
    }

    private void LoadBricks()
    {
        List<GameState.BrickState> savedBricks = GameState.GetSavedBricks();
        foreach (GameState.BrickState obj in savedBricks)
        {
            GameObject newBrick = Instantiate(brick, obj.position.ToVector3(), Quaternion.identity);
            newBrick.GetComponent<Brick>().SetHP(obj.hp, obj.maxHP);
        }
    }

    private void SaveState()
    {
        GameObject[] allBricks = GameObject.FindGameObjectsWithTag("Brick");
        GameObject shootPosition = GameObject.FindGameObjectWithTag("Player");
        List<GameState.BrickState> allBricksState = new List<GameState.BrickState>();

        foreach (GameObject obj in allBricks)
        {
            allBricksState.Add(new GameState.BrickState(obj));
        }

        GameState.SaveBricks(allBricksState);
        GameState.SavePosition(shootPosition.transform.position);
        GameState.SaveCountBalls(shootPosition.GetComponent<PlayerCotrol>().GetCountBalls());
        GameState.SaveHpNewBricks(hpBricks);

        SaveLoad.SaveGame();
    }

    private void SpawnNewVawe(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            Vector3 newBrickPosition = BrickPosition.GetBrickCoord(Random.Range(0, 4), Random.Range(1, 6));
            GameObject newBrick = Instantiate(brick, newBrickPosition, Quaternion.identity);
            newBrick.GetComponent<Brick>().SetHP(hpBricks, hpBricks);
        }
    }

    private void OnDestroyAllBalls()
    {
        hpBricks++;
        SpawnNewVawe(CountNewBricks);
        SaveState();
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

}
