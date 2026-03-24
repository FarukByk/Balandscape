using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardSystem : MonoBehaviour
{
    public GameObject cardPref;
    public List<cardInfo> cards;
    List<cardInfo> list = new List<cardInfo>(0);
    public void Awake()
    {
        foreach (cardInfo card in cards)
        {
            for (int i = 0; i < card.oran; i++)
            {
                list.Add(card);
            }
        }
    }
    public GameObject GetCard()
    {
        GameObject go = Instantiate(cardPref);
        int a = UnityEngine.Random.Range(0, list.Count);
        go.GetComponent<card>().cardName = list[a].name;
        go.GetComponent<card>().image = list[a].image;
        go.GetComponent<card>().obj = list[a].obj;
        go.GetComponent<card>().change = list[a].change;
        go.GetComponent<card>().cardMode = list[a].cardMode;
        return go;
    }

    public GameObject GetCard(int index)
    {
        GameObject go = Instantiate(cardPref);
        int a = index;
        go.GetComponent<card>().cardName = cards[a].name;
        go.GetComponent<card>().image = cards[a].image;
        go.GetComponent<card>().obj = cards[a].obj;
        go.GetComponent<card>().change = cards[a].change;
        go.GetComponent<card>().cardMode = cards[a].cardMode;
        return go;
    }

}

[Serializable]
public class cardInfo
{
    public string name;
    public Sprite image;
    public GameObject obj;
    public int cardMode;
    public int change = -1;
    public int oran;
}