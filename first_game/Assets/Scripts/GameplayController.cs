using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject brick;
    public int CountNewBricks = 8;
    public GameObject leftBorder;
    public GameObject rightBorder;
    public GameObject topBorder;
    public GameObject bottomBorder;

    private float gameClipPlane;
    private int hpBricks = 1;

    private void Awake()
    {
        gameClipPlane = Camera.main.nearClipPlane + 2f;
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
        leftBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, gameClipPlane));
        rightBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, gameClipPlane));
        topBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, gameClipPlane));
        bottomBorder.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.05f, gameClipPlane));
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
        HashSet<BrickPosition.position> position = new HashSet<BrickPosition.position>();

        while (position.Count < count)
        {
            position.Add(new BrickPosition.position(Random.Range(0, 4), Random.Range(1, 6)));
        }

        foreach (BrickPosition.position pos in position)
        {
            Vector3 newBrickPosition = BrickPosition.GetBrickCoord(pos.GetSide(), pos.GetPosition());
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
