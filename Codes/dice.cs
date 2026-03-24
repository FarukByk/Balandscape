using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dice : MonoBehaviour
{
    Animator animator;
    public int number;
    public List<Quaternion> rot = new List<Quaternion>(6);
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("hit");
            number = Random.Range(1, 7);
            transform.Find("d").Find("model").localRotation = rot[number-1];
        }
    }
}
