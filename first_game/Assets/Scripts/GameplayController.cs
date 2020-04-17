using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameObject brick;
    public GameObject borders;
    public GameObject bullet;
    public GameObject player;
    public float cooldown = 0.5f;
    public int countSpawnBricks = 6;

    private int countBalls = 1;
    private int hpBricks = 1;
    private float leftBorderSpawn;
    private float rightBorderSpawn;
    private float topBorderSpawn;
    private float widthBrick;
    private int countPlaceForSpawn;

    private float timeNextShoot = 0f;
    private int countBallsOnShoot;
    private bool readyShoot = true;
    private bool canAim = true;
    private GameObject arrow;

    void Start()
    {
        SetBorders(); //position for collision borders

        //set border for spawn area
        leftBorderSpawn = Camera.main.ViewportToWorldPoint(new Vector3(0.01f, 0f)).x;
        rightBorderSpawn = Camera.main.ViewportToWorldPoint(new Vector3(0.99f, 0f)).x;
        topBorderSpawn = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.95f)).y;

        widthBrick = brick.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

        countPlaceForSpawn = (int)((Mathf.Abs(leftBorderSpawn) + rightBorderSpawn) / widthBrick) - 1;

        if (SaveLoad.LoadGame())
        {
            player.transform.position = GameState.GetSavedPosition().ToVector3();
            LoadBricks();
            countBalls = GameState.GetCountBalls();
            hpBricks = GameState.GetHpNewBricks();
        }
        else
        {
            SpawnBricks();
            player.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0f));
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
    }

    countBallsOnShoot = countBalls;

        arrow = player.transform.Find("Arrow").gameObject;

        GameEvents.onDestroyAllBalls.AddListener(OnDestroyAllBalls);
    }

    void Update()
    {
        if (canAim && Input.GetButton("Fire1"))
            Aiming();
        Shoot();
    }

    public void SpawnBricks()
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

        for (int i = 0; i < countSpawnBricks; i++)
        {
            GameObject newBrick = Instantiate(brick, listPlaces[i], Quaternion.identity);
            newBrick.GetComponent<Brick>().SetHP(hpBricks);
        }

        hpBricks++;        
    }

    private void LoadBricks()
    {
        List<GameState.BrickState> savedBricks = GameState.GetSavedBricks();
        foreach (GameState.BrickState obj in savedBricks)
        {
            GameObject newBrick = Instantiate(brick, obj.position.ToVector3(), Quaternion.identity);
            newBrick.GetComponent<Brick>().SetHP(obj.hp);
        }
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

    private void SetBorders()
    {
        borders.transform.Find("leftBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f));
        borders.transform.Find("rightBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f));
        borders.transform.Find("topBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f));
        borders.transform.Find("bottomBorder").position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.03f));
    }

    private void Shoot()
    {
        if (Input.GetButtonUp("Fire1") && readyShoot)
        {
            canAim = false;
            readyShoot = false;
            arrow.SetActive(false);
            InstantiateBall();
        }
        else if (timeNextShoot < Time.time && countBallsOnShoot > 0 && !readyShoot)
            InstantiateBall();
    }

    private void InstantiateBall()
    {
        timeNextShoot = Time.time + cooldown;
        countBallsOnShoot--;
        if (PoolBalls.CountInactive() == 0)
        {
            GameObject ball = Instantiate(bullet, player.transform.position, player.transform.rotation);
            PoolBalls.Add(ball);
        }
        else
        {
            PoolBalls.ActiveNext(player.transform.position, player.transform.rotation);
        }
    }

    private void Aiming()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mousePos);
        if (hit.collider != null)
        {
            arrow.GetComponent<LineRenderer>().SetPosition(0, player.transform.position);
            arrow.GetComponent<LineRenderer>().SetPosition(1, hit.point);
            arrow.SetActive(true);
        }

        if (mousePos != Vector2.zero)
        {
            float angle = Mathf.Atan2(-mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            player.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void SaveState()
    {
        GameObject[] allBricks = GameObject.FindGameObjectsWithTag("Brick");
        List<GameState.BrickState> allBricksState = new List<GameState.BrickState>();

        foreach (GameObject obj in allBricks)
        {
            allBricksState.Add(new GameState.BrickState(obj));
        }

        GameState.SaveBricks(allBricksState);
        GameState.SavePosition(player.transform.position);
        GameState.SaveCountBalls(countBalls);
        GameState.SaveHpNewBricks(hpBricks);

        SaveLoad.SaveGame();
    }

    private void OnDestroyAllBalls()
    {
        countBalls++;
        countBallsOnShoot = countBalls;
        readyShoot = true;
        canAim = true;
        player.transform.position = new Vector2(PoolBalls.NextPosition(), player.transform.position.y);

        Invoke("SpawnBricks", 0.1f);

        Invoke("SaveState", 0.2f);
    }
}
