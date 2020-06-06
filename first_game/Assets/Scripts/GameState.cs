using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class GameState
{
    private static List<BrickState> savedBricks = new List<BrickState>();
    private static Vec3 shootPosition = new Vec3();
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
    public struct Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vector3 vec3)
        {
            x = vec3.x;
            y = vec3.y;
            z = vec3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    [System.Serializable]
    public struct BrickState
    {
        public Vec3 position;
        public int hp;
        public int maxHP;

        public BrickState(int hp, int maxHP, Vec3 position)
        {
            this.hp = hp;
            this.maxHP = maxHP;
            this.position = position;
        }
        public BrickState(GameObject go)
        {
            hp = go.GetComponent<Brick>().GetHP();
            maxHP = go.GetComponent<Brick>().GetMaxHP();
            position = new Vec3(go.transform.position);
        }
    }

    [System.Serializable]
    public struct AllState
    {
        public List<BrickState> listBricks;
        public Vec3 posPlayer;
        public int countBalls;
        public int hpNewBricks;

        public AllState(List<BrickState> listBricks, Vec3 posPlayer, int countBalls, int hpNewBricks)
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

    public static void SavePosition(Vector3 pos)
    {
        shootPosition = new Vec3(pos);
    }

    public static Vec3 GetSavedPosition()
    {
        return shootPosition;
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
        shootPosition = state.posPlayer;
        countBalls = state.countBalls;
        hpNewBricks = state.hpNewBricks;
    }

    public static AllState SaveState()
    {
        return new AllState(savedBricks, shootPosition, countBalls, hpNewBricks);
    }
}
