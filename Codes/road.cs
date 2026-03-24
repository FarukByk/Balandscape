using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class road : MonoBehaviour
{
    Transform mainRot;
    public bool[] pBool = new bool[4];
    public string objectName;
    GameObject[] yols = new GameObject[4];
    int x = 0;
    house h;
    
    private void Start()
    {
        h = GetComponent<house>();
        mainRot = transform.Find("model");

        for (int i = 0; i < 4; i++)
        {
            yols[i] = mainRot.Find(objectName + i.ToString()).gameObject;
        }
        change();
        mainCode.OnMove += change;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnDestroy()
    {
        mainCode.OnMove -= change;
    }
    void change()
    {
        pBool[0] = false;
        pBool[1] = false;
        pBool[2] = false;
        pBool[3] = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        int a = 0;
        for (int i = 0; i < pBool.Length; i++)
        {
            if (i == 0 && myMath.check(h.pos + (Vector3.right*2)) == objectName)
            {
                pBool[i] = true;
            }
            if (i == 1 && myMath.check(h.pos + (Vector3.back*2)) == objectName)
            {
                pBool[i] = true;
            }
            if (i == 2 && myMath.check(h.pos + (Vector3.left * 2)) == objectName)
            {
                pBool[i] = true;
            }
            if (i == 3 && myMath.check(h.pos + (Vector3.forward * 2)) == objectName)
            {
                pBool[i] = true;
            }
            if (pBool[i])
            {
                a++;
            }
        }
        if (a == 0 || a == 1)
        {
            x = 0;
            if (pBool[0] || pBool[2])
            {
                mainRot.localRotation = Quaternion.Euler(0, 90, 0);
            }
            if (pBool[1] || pBool[3])
            {
                mainRot.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (a == 2)
        {
            int se = 0;
            bool b = false;
            for (int i = 0; i < pBool.Length; i++)
            {
                if (i != 3)
                {
                    if (pBool[i] && pBool[i + 1])
                    {
                        b = true;
                        se = i + 1;
                    }
                }
                else
                {
                    if (pBool[i] && pBool[0])
                    {
                        b = true;
                        se = 0;
                    }
                }
                
            }
            
            if (b)
            {
                x = 3;
                mainRot.localRotation = Quaternion.Euler(0, se*90, 0);
            }
            else
            {
                x = 0;
                if (pBool[0] || pBool[2])
                {
                    mainRot.localRotation = Quaternion.Euler(0, 90, 0);
                }
                if (pBool[1] || pBool[3])
                {
                    mainRot.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }

        }
        else if (a == 3)
        {
            x = 1;
            int se = 0;
            for (int i = 0; i < pBool.Length; i++)
            {
                if (!pBool[i])
                {
                    se = i;
                }
            }
            mainRot.localRotation = Quaternion.Euler(0, se * 90, 0);
        }
        else if (a == 4)
        {
            x = 2;
        }
        for (int i = 0; i < pBool.Length; i++)
        {
            if (i == x)
            {
                yols[i].SetActive(true);
            }
            else
            {
                yols[i].SetActive(false);

            }
        }
        
    }
}
