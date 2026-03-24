using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    planor myplanor;
    public int gold,pop;
    public TMP_Text goldText,popText;
    public List<card> myCards;
    public int selectedIndex;
    cardSystem cardSys;
    target target;
    [HideInInspector] public bool open;
    mainCode mc;
    float distance = 200;
    private void Start()
    {
        myplanor = FindAnyObjectByType<planor>();
        mc = FindAnyObjectByType<mainCode>();
        target = FindAnyObjectByType<target>();
        selectedIndex = -1;
        cardSys = GetComponent<cardSystem>();
        myMath.waitAndStart(1, () => { RandomCard(5); });
    }
    void Update()
    {
        #region popFind
        int def = 0;
        foreach (house h in mc.houses)
        {
            def += h.pop;
        }
        pop = def;
        #endregion
        selectedIndex = Mathf.Clamp(selectedIndex, -1, myCards.Count - 1);
        #region cardChange
        if (selectedIndex > -1)
        {
            if (target.active)
            {
                target.mode = myCards[selectedIndex].cardMode;
            }
            else
            {
                target.mode = -1;
            }
            if (myCards[selectedIndex].change > -1 && Input.GetKeyDown("r"))
            {
                addCard(cardSys.GetCard(myCards[selectedIndex].change));
                useCard();                
                selectedIndex = myCards.Count - 1;
            }
        }
        else
        {
            target.mode = -1;
        }
            
        #endregion


        if (open)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,0,0), Time.deltaTime * 5);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -150, 0), Time.deltaTime * 5);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 4 / 5 ,Time.deltaTime*5);
        }
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].cardIndex = i;
            if (myCards[i].inventory == null)
            {
                myCards[i].inventory = this;
            }
        }

        float sex = -(distance * (myCards.Count-1)/2);
        
        if (selectedIndex != -1)
        {
            for (int i = 0; i < myCards.Count; i++)
            {
                if (selectedIndex == i)
                {
                    myCards[i].transform.localScale = Vector3.Lerp(myCards[i].transform.localScale, new Vector3(1.5f, 1.5f, 1), Time.deltaTime * 10);
                    myCards[i].transform.localPosition = Vector3.Lerp(myCards[i].transform.localPosition, new Vector3(i * distance + sex, 240, 0), Time.deltaTime * 10);
                    myCards[i].transform.SetAsLastSibling();
                }
                else
                {
                    myCards[i].transform.localScale = Vector3.Lerp(myCards[i].transform.localScale, Vector3.one, Time.deltaTime * 10);
                    myCards[i].transform.localPosition = Vector3.Lerp(myCards[i].transform.localPosition, new Vector3(i * distance + sex, 160, 0), Time.deltaTime * 10);
                }
            }
        }
        else 
        { 

            for (int i = 0; i < myCards.Count; i++)
            {
                myCards[i].transform.localScale = Vector3.Lerp(myCards[i].transform.localScale, Vector3.one, Time.deltaTime * 10);
                myCards[i].transform.localPosition = Vector3.Lerp(myCards[i].transform.localPosition, new Vector3(i * distance + sex, 160, 0), Time.deltaTime * 10);
            }
                
        }
    }
    private void LateUpdate()
    {
        goldText.text = gold.ToString() + ">";
        popText.text = pop.ToString() + "@";
    }

    public void addGold(int value)
    {
        gold += value;
    }
    public void openClose(bool open)
    {
        this.open = open;
    }
    public void RandomCard(int cardCount)
    {
        for (int i = 0; i < cardCount; i++)
        {
            addCard(cardSys.GetCard());
        }
    }
    public house getCardObject()
    {
        return myCards[selectedIndex].obj.GetComponent<house>();
    }

    public bool useCard()
    {
        if (myCards[selectedIndex] != null)
        {
            Destroy(myCards[selectedIndex].gameObject,1);
            myCards[selectedIndex].ded();
            myCards.RemoveAt(selectedIndex);
            if (myCards.Count == 0)
            {
                myMath.waitAndStart(1, () => { RandomCard(5); });
                myplanor.droped();
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void addCard(GameObject card)
    {
        GameObject a = Instantiate(card);
        a.transform.SetParent(transform);
        myCards.Add(a.GetComponent<card>());
    }

    
}
