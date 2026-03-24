using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour
{
    public string cardName;
    public Sprite image;
    [HideInInspector] public int cardIndex;
    [HideInInspector] public Inventory inventory;
    public GameObject obj;
    public int cardMode;
    public int change = -1;
    Animator animator;

    public void Start()
    {
        house h = obj.GetComponent<house>();
        animator = GetComponent<Animator>();
        transform.Find("Image").Find("nameText").GetComponent<TMP_Text>().text = cardName;
        transform.Find("Image").Find("infos").GetComponent<TMP_Text>().text = h.pop == 0? ($"{h.weight}<") : h.mode == 0? ($"{h.pop}@      {h.weight}<") : ($"{h.pop}x{h.mode+1}@      {h.weight}x{h.mode+1}<");
        transform.Find("Image").Find("mini").GetComponent<Image>().sprite = image;
    }

    public void select()
    {
        inventory.selectedIndex = cardIndex;
        animator.SetTrigger("hit");
    }   

    public void ded()
    {
        animator.SetTrigger("death");
    }
}
