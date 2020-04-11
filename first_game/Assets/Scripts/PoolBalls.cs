using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBalls : MonoBehaviour
{
    private static List<GameObject> pool = new List<GameObject>();

    public static void Add(GameObject obj)
    {
        pool.Add(obj);
    }

    public static void Remove(GameObject obj)
    {
        Destroy(obj.gameObject);
        pool.Remove(obj);
    }

    public void SetActive(bool active, GameObject obj)
    {
        pool[pool.IndexOf(obj)].SetActive(active);
    }

    public static void Inactivate(GameObject obj)
    {
        int index = pool.IndexOf(obj);
        pool[index].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        pool[index].SetActive(false);
        if (CountActive() == 0)
            GameEvents.onDestroyAllBalls.Invoke();
    }

    public static GameObject ActiveNext(Vector2 startCoord)
    {
        if (pool.Count > 0)
        {
            foreach (GameObject obj in pool)
            {
                if (obj.activeSelf == false)
                {
                    obj.GetComponent<Bullet>().NewStart(startCoord);
                    obj.SetActive(true);
                    return obj;
                }
            }
            return null;
        }
        else
        {
            return null;
        }

    }

    public static int Count()
    {
        return pool.Count;
    }

    public static int CountInactive()
    {
        if (pool.Count > 0)
        {
            int CountInactive = 0;
            foreach (GameObject obj in pool)
            {
                if (obj.activeSelf == false)
                    CountInactive++;
            }
            return CountInactive;
        }
        else
        {
            return 0;
        }
    }

    public static int CountActive()
    {
        if (pool.Count > 0)
        {
            int CountActive = 0;
            foreach (GameObject obj in pool)
            {
                if (obj.activeSelf == true)
                    CountActive++;
            }
            return CountActive;
        }
        else
        {
            return 0;
        }
    }

}
