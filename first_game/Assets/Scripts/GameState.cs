using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class GameState
{
    private static List<BrickState> savedBricks = new List<BrickState>();
    private static Vec2 startPosition = new Vec2();
    private static int countBalls;
    private static int hpNewBricks;

    [System.Serializable]
    public struct Vec2
    {
        public float x, y;

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2(Vector2 vec2)
        {
            x = vec2.x;
            y = vec2.y;
        }

        public Vec2(Vector3 vec3)
        {
            x = vec3.x;
            y = vec3.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(this.x, this.y);
        }

        public Vector2 ToVector3()
        {
            return new Vector3(this.x, this.y, Vector3.zero.z);
        }
    }

    [System.Serializable]
    public struct BrickState
    {
        public Vec2 position;
        public int hp;

        public BrickState(int hp, Vec2 position)
        {
            this.hp = hp;
            this.position = position;
        }
        public BrickState(GameObject go)
        {
            hp = go.GetComponent<Brick>().GetHP();
            position = new Vec2(go.transform.position);
        }
    }

    [System.Serializable]
    public struct AllState
    {
        public List<BrickState> listBricks;
        public Vec2 posPlayer;
        public int countBalls;
        public int hpNewBricks;

        public AllState(List<BrickState> listBricks, Vec2 posPlayer, int countBalls, int hpNewBricks)
        {
            this.listBricks = listBricks;
            this.posPlayer = posPlayer;
            this.countBalls = countBalls;
            this.hpNewBricks = hpNewBricks;
        }
    }

    public static void ClearState()
    {
        savedBricks.Clear();
        startPosition = new Vec2(0, 0);
    }

    public static void SaveStateBrick(GameObject brick)
    {
        savedBricks.Add(new BrickState(brick));
    }

    public static List<BrickState> GetSavedBricks()
    {
        return savedBricks;
    }
    public static void SaveBricks(List<BrickState> listBricks)
    {
        savedBricks = listBricks;
    }

    public static void SavePosition(Vector2 pos)
    {
        startPosition = new Vec2(pos);
    }

    public static Vec2 GetSavedPosition()
    {
        return startPosition;
    }

    public static void SaveCountBalls(int count)
    {
        countBalls = count;
    }

    public static int GetCountBalls()
    {
        return countBalls;
    }

    public static void SaveHpNewBricks(int count)
    {
        hpNewBricks = count;
    }

    public static int GetHpNewBricks()
    {
        return hpNewBricks;
    }

    public static void LoadState(AllState state)
    {
        savedBricks = state.listBricks;
        startPosition = state.posPlayer;
        countBalls = state.countBalls;
        hpNewBricks = state.hpNewBricks;
    }

    public static AllState SaveState()
    {
        return new AllState(savedBricks, startPosition, countBalls, hpNewBricks);
    }
}
