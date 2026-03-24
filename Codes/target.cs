using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    public bool delete;
    public int mode;
    public bool active;
    GameObject[] objs = new GameObject[4];
    [HideInInspector] public int rot = 0;
    GameObject del;
    Transform t;
    void Start()
    {
        t = transform.Find("t");
        del = transform.Find("delete").gameObject;
        objs = new GameObject[t.transform.childCount];
        for (int i = 0; i< t.transform.childCount; i++)
        {
            objs[i] = t.Find(i.ToString()).gameObject;
        }
    }

    
    void Update()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            if (i <= mode)
            {
                objs[i].SetActive(true);
            }
            else
            {
                objs[i].SetActive(false);
            }
        }
        t.gameObject.SetActive(!delete);
        del.SetActive(delete);
        if (Input.GetMouseButtonDown(1))
        {
            rot += 90;
            rot = rot % 360;
            t.transform.localRotation = Quaternion.Euler(0, rot, 0);
        }
    }
}
