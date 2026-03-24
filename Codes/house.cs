using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class house : MonoBehaviour
{
    public string typeName;
    public int weight;
    public int pop;
    public Vector3 pos;
    public bool isHouse;
    public int mode = 0;
    public bool upgrade;
    public List<Vector3> extras;
    public GameObject extraObject;
    [HideInInspector]public List<house> connectedObjects = new List<house>();
    [SerializeField]public need[] needs = new need[0];
    void Start()
    {
        mySoundSystem.PlaySoundEffect("put", transform.position);
        myMath.destroyParticle(transform, "putParticle");
        if (transform.Find("plane") != null)
        {
            transform.Find("plane").GetComponent<plane>().pos = transform.Find("plane").localPosition + pos;
        }
        if (isHouse)
        {
            
            for (int i = 0; i < 4; i++)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("window" + (myMath.possible(20) ? "1" : "0")));
                go.transform.parent = transform.Find("model");
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.Euler(-90, 0, i * 90);
                go.transform.localScale = Vector3.one * 100;
            }
            
        }
        if (TryGetComponent<rooftop>(out rooftop a))
        {
            a.Check();
        }
        
    }
    public void delete()
    {
        myMath.destroyParticle(transform, "destroyParticle");
        FindAnyObjectByType<mainCode>().houses.Remove(this);
        Destroy(gameObject);
        myMath.balance();
        foreach (house h in connectedObjects)
        {
            if (h != null)
            {
                h.connectedObjects.Remove(this);
                h.StartCoroutine(waitDel(0.2f, h));
            }
        }
        
    }
    public bool checkNeeds(Vector3 pos,bool notRoadCheck)
    {
        return checkNeeds0(pos);
    }
    public bool checkNeeds(Vector3 pos)
    {
        return checkNeeds1(pos);
    }
    
    bool checkNeeds1(Vector3 pos)
    {
        bool s = false;
        if (pos.y == 0 && !upgrade)
        {
            if (myMath.check(pos + new Vector3(2, 0, 0)) == "road" || myMath.check(pos + new Vector3(-2, 0, 0)) == "road" || myMath.check(pos + new Vector3(0, 0,2)) == "road" || myMath.check(pos + new Vector3(0, 0, -2)) == "road")
            {
                s = true;
            }
        }
        else if(typeName != "road")
        {
            
            foreach (need n in needs)
            {
                if (!n.not)
                {
                    if (myMath.check(pos + n.pos) == n.name)
                    {
                        s = true;
                    }
                }
                else
                {
                    if (myMath.check(pos + n.pos) == n.name)
                    {
                        s = false;
                    }
                }
            }
        }

        return s;
    }
    bool checkNeeds0(Vector3 pos)
    {
        bool s = false;
        if (pos.y != 0)
        {
            foreach (need n in needs)
            {
                if (!n.not)
                {
                    if (myMath.check(pos + n.pos) == n.name)
                    {
                        s = true;
                    }
                }
                else
                {
                    if (myMath.check(pos + n.pos) == n.name)
                    {
                        s = false;
                    }
                }
            }

        }
        else
        {
            s = true;
        }
        return s;
    }

    IEnumerator waitDel(float sec, house h)
    {
        yield return new WaitForSeconds(sec);
        h.delete();
    }
}
[Serializable]
public class need
{
    public bool not;
    public Vector3 pos;
    public string name;
}