using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class rooftop : MonoBehaviour
{
    house myHouse;
    GameObject textEf;
    public float multiple = 1;
    private void Start()
    {
        
    }
    public void Check()
    {
        textEf = Resources.Load<GameObject>("textEf");
        myHouse = GetComponent<house>();
        int pop = 0;
        house sa = myMath.getHouse(new Vector3(myHouse.pos.x,0, myHouse.pos.z));
        while (sa != null)
        {
            pop += sa.pop;
            sa = myMath.getHouse(sa.pos + new Vector3(0,2,0));
        }

        myHouse.pop =(int)(pop * (multiple - 1));
        addPop((int)(pop * (multiple - 1)));
    }
    void addPop(int miktar)
    {
        GameObject a = Instantiate(textEf);
        Destroy(a, 1);
        a.transform.position =transform.position + (Vector3.up * 2);
        a.transform.Find("Canvas").Find("t").Find("Text").GetComponent<TMP_Text>().text = (miktar).ToString() + "@";
    }


}
