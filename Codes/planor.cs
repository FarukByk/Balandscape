using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planor : MonoBehaviour
{
    Animator animator;
    int s;
    public int time = 8;
    mainCode mc;
    plane targetPlane;
    public house obj;
    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCode.OnMove += change;
        mc = FindAnyObjectByType<mainCode>();
        s = mc.moveCount % time;
    }

    void change()
    {
        if ((mc.moveCount - s) % time == 0 && (mc.moveCount - s) != 0)
        {
            //droped();
        }
    }
    public void droped()
    {
        time = 5;
        animator.SetTrigger("fly");
        targetPlane = mc.planes[Random.Range(0, mc.planes.Count)];
        transform.position = targetPlane.gameObject.transform.position;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
    public void OnDestroy()
    {
        mainCode.OnMove -= change;
    }
    public void drop()
    {
        mc.DeleteAndAddObject(targetPlane, obj);
    }
}
