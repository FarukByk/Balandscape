using System;
using UnityEngine;
using System.Threading.Tasks;
public class myMath : MonoBehaviour
{
    public static void destroyParticle(Transform tr , string name)
    {
        mainCode main = FindAnyObjectByType<mainCode>();
        ParticleSystem ps = main.transform.Find(name).GetComponent<ParticleSystem>();
        ps.transform.position = tr.position;
        ps.transform.rotation = tr.rotation;
        ps.Play();
    }
    public static bool possible(int x)
    {
        int a = UnityEngine.Random.Range(0, 100);
        if (a < x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void balance()
    {
        mainCode main = FindAnyObjectByType<mainCode>();
        main.balance();
    }
    public static string check(Vector3 pos)
    {
        mainCode main = FindAnyObjectByType<mainCode>();
        string a = "null";
        foreach (house h in main.houses)
        {
            if (h.pos == pos)
            {
                a = h.typeName;
                break;
            }
        }
        return a;

       
    }

    public static house getHouse(Vector3 pos)
    {
        mainCode main = GameObject.Find("main").GetComponent<mainCode>();
        house a = null;
        foreach (house h in main.houses)
        {
            if (h.pos == pos)
            {
                a = h;
                break;
            }
        }
        return a;
    }

    public static Vector3 rotxvec3(Vector3 pos, float rot)
    {
        if (rot == 0)
        {
            return pos;
        }
        else if (rot == 90)
        {
            Vector3 vec = new Vector3(pos.z,0,-pos.x);
            return vec;
        }
        else if (rot == 180 || rot == -180)
        {
            Vector3 vec = new Vector3(-pos.x, 0, -pos.z);
            return vec;
        }
        else if (rot == 270 ||rot == -90)
        {
            Vector3 vec = new Vector3(-pos.z, 0, pos.x);
            return vec;
        }
        else
        {
            return pos;
        }
    }

    public static bool checkRoads(Vector3 pos)
    {
        bool s = false;
        if (pos.y == 0)
        {
            if (myMath.check(pos + new Vector3(2, 0, 0)) == "road" || myMath.check(pos + new Vector3(-2, 0, 0)) == "road" || myMath.check(pos + new Vector3(0, 0, 2)) == "road" || myMath.check(pos + new Vector3(0, 0, -2)) == "road")
            {
                s = true;
            }
        }
        return s;
    }

    public static bool checkColumn(Vector3 pos)
    {   
        return check(new Vector3(pos.x, 0, pos.z)) != "null";
    }
    public static async void waitAndStart(float second, Action action)
    {
        await Task.Delay((int)(second * 1000));
        action?.Invoke();
    }



    public static void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static float LoadFloat(string key,float value = 0)
    {
        if (HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            SaveFloat(key, 0);
            return value;
        }
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

}
