using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class factory : MonoBehaviour
{
    Transform top;
    public int x = 1;
    house house;
    public int time, gold;
    public Image bar;
    int s;
    house myhouse;
    mainCode mc;
    GameObject textEf;
    public int upgradeWeight;
    void Start()
    {
        myhouse = GetComponent<house>();
        textEf = Resources.Load<GameObject>("textEf");
        top = transform.Find("model").Find("top");
        house = GetComponent<house>();
        mainCode.OnMove += change;
        mc = FindAnyObjectByType<mainCode>();
        s = mc.moveCount % time;
        if (house.pos.y != 0)
        {
            foreach (Transform t in transform.Find("model"))
            {
                if (t.gameObject.activeSelf)
                {
                    Destroy(t.gameObject);
                }
                else
                {
                    t.gameObject.SetActive(true);
                }
            }
            house.weight = upgradeWeight;
            Destroy(this);
        }
    }
    public void OnDestroy()
    {
        mainCode.OnMove -= change;
    }
    private void Update()
    {
        
        float deg = ((float)mc.moveCount - s + 1) % time;
        float xd = deg / (time - 1);
        bar.fillAmount = Mathf.Lerp(bar.fillAmount, xd, Time.deltaTime);
    }
    void change()
    {
        
        int a = 1;
        while(myMath.check(new Vector3(house.pos.x,a*2,house.pos.z)) == myhouse.typeName)
        {
            a++;
        }
        x = a;
        top.localPosition = new Vector3(0, x*2, 0);
        if ((mc.moveCount -s +1 )% time == 0)
        {
            addGold();
            bar.fillAmount = 0;
        }
        
    }

    void addGold()
    {
        mc.inventory.addGold(gold*x);
        GameObject a = Instantiate(textEf);
        Destroy(a,1);
        a.transform.position = top.position + (Vector3.up*3);
        a.transform.Find("Canvas").Find("t").Find("Text").GetComponent<TMP_Text>().text =  (gold*x).ToString() + ">";
    }
}
