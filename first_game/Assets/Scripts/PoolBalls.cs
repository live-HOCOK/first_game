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

    public static void SetActive(bool active, GameObject obj)
    {
        pool[pool.IndexOf(obj)].SetActive(active);
    }

    public static void Inactivate(GameObject obj)
    {
        if (pool.Count == 0)
            Add(obj);
        int index = pool.IndexOf(obj);
        pool[index].GetComponent<Rigidbody>().velocity = Vector3.zero;
        pool[index].SetActive(false);
        if (CountActive() == 0)
            GameEvents.onDestroyAllBalls.Invoke();
    }

    public static GameObject ActiveNext(Vector3 startCoord, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            foreach (GameObject obj in pool)
            {
                if (obj.activeSelf == false)
                {
                    obj.SetActive(true);
                    obj.GetComponent<Bullet>().NewStart(startCoord, rotation);
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

    public static float NextPosition()
    {
        
        float[] poolCoord = new float[pool.Count];

        int temp = 0;
        foreach (GameObject obj in pool) //fill array of coord
        {
            poolCoord[temp] = ((float)(decimal.Round(System.Convert.ToDecimal(obj.transform.position.x), 2)));
            temp++;
        }

        int[] count = new int[poolCoord.Length]; //array containing counted matches 

        for (int i = 0; i < poolCoord.Length; i++) //find matches
        {
            for (int ii = 0; ii < poolCoord.Length; ii++)
            {
                if (poolCoord[i] == poolCoord[ii])
                    count[i]++;
            }
        }

        return poolCoord[IndexMaxInArray(count)];
    }

    private static int IndexMaxInArray(int[] array) //find index max value in array
    {
        int max = 0;
        foreach (int i in array)
            if (i > max)
                max = i;
        for (int i = 0; i < array.Length; i++)
            if (array[i] == max)
                return i;
        return 0;
    }
}