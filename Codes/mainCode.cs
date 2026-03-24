using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainCode : MonoBehaviour
{
    public int moveCount;
    public static Action OnMove;
    public int worldScale = 8;
    public List<house> houses;
    [HideInInspector] public List<plane> planes;
    public float rotx, rotz;
    public float balanceStrength;
    public GameObject AddScoreTable;
    bool starting;
    public TMP_InputField PlayerName;
    [HideInInspector]public Inventory inventory;
    ParticleSystem fallWater;
    bool finish;
    void Start()
    {
        fallWater = transform.Find("fallWaterParticle").GetComponent<ParticleSystem>();
        CreateWorld();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        myMath.waitAndStart(2, () =>
        {
            fromStart(new Vector3(1, 0, 1));
            fromStart(new Vector3(-1, 0, 1));
            fromStart(new Vector3(1, 0, -1));
            fromStart(new Vector3(-1, 0, -1));
        });
    }
    Vector3 moveVector = Vector3.zero;
    float fallSpeed = 15;
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            finish = true;
            SceneManager.LoadScene("lobby");
        }
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(rotx,0,rotz), Time.deltaTime*5);
        if (finish)
        {
            bool isFinished = true;
            foreach (house h in houses)
            {
                if (h != null)
                {
                    Transform t = h.transform;
                    isFinished = false;
                    if (t.localPosition.x >= 17 || t.localPosition.x <= -17 || t.localPosition.z >= 17 || t.localPosition.z <= -17)
                    {
                        t.localPosition += moveVector * Time.deltaTime * fallSpeed/3;
                        t.position += Vector3.down * fallSpeed * Time.deltaTime*2;
                        if (t.position.y <= 0)
                        {
                            fallWater.transform.position = t.position;
                            fallWater.transform.rotation = Quaternion.Euler(0, 0, 0);
                            fallWater.Play();
                            Destroy(h.gameObject);
                        }
                    }
                    else
                    {
                        t.localPosition += moveVector * Time.deltaTime * fallSpeed;
                    }
                }
                
            }
            if (isFinished)
            {
                StartCoroutine(scoreTableAdd());
            }
            
        }
        
    }


    public void Fin()
    {    
        if (!string.IsNullOrEmpty(PlayerName.text))
        {
            Inventory inv = FindAnyObjectByType<Inventory>();
            Debug.Log(PlayerName.text + inv.pop.ToString());
            leaderBoard.addSocre(PlayerName.text,inv.pop);

            SceneManager.LoadScene("lobby");
        }
        

    }


    IEnumerator scoreTableAdd()
    {
        yield return new WaitForSeconds(2);
        
        AddScoreTable.SetActive(true);
    }
   
    void CreateWorld()
    {
        
        GameObject plane = Resources.Load<GameObject>("plane");
        Vector3 pos = -new Vector3((worldScale*2) -1, 0, (worldScale * 2) - 1);
        for (int x = 0; x < worldScale*2; x++)
        {
            for (int y = 0; y < worldScale*2; y++)
            {
                GameObject go = Instantiate(plane);
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(x * 2, 0, y * 2) + pos;
                go.transform.localRotation = Quaternion.Euler(0, 0, 0);
                go.GetComponent<plane>().pos = new Vector3(x * 2, 0, y * 2) + pos;
                planes.Add(go.GetComponent<plane>());
            }
        }
        
    }

    public void balance()
    {
        float x = 0;
        float z = 0;
        foreach (house h in houses)
        {
            x += h.pos.x * h.weight * balanceStrength;
            z += h.pos.z * h.weight * balanceStrength;
        }
        rotx = z;
        rotz = -x;
        rotx = Mathf.Clamp(rotx, -31, 31);
        rotz = Mathf.Clamp(rotz, -31, 31);
        if (rotx >= 30)
        {
            finalTheGame(new Vector3(0, 0, 1));
        }
        else if (rotx <= -30)
        {
            finalTheGame(new Vector3(0, 0, -1));
        }
        else if(rotz >= 30)
        {
            finalTheGame(new Vector3(-1, 0, 0));
        }
        else if (rotz <= -30)
        {
            finalTheGame(new Vector3(1, 0, 0));
        }
    }

    public void DeleteObject(plane plane)
    {
        if (myMath.checkColumn(plane.pos))
        {
            house h = myMath.getHouse(new Vector3(plane.pos.x, 0, plane.pos.z));
            if (h != null)
            {
                h.delete();
            }
            StartCoroutine(wait());
            moveCount++;
        }
        
    }

    public void DeleteAndAddObject(plane plane , house obj)
    {
        if (myMath.checkColumn(plane.pos))
        {
            house h = myMath.getHouse(new Vector3(plane.pos.x, 0, plane.pos.z));
            if (h != null)
            {
                h.delete();
            }
        }
        AddObject(plane, 0, null,obj);
    }
    public void AddObject(plane plane,float rot,house connect)
    {
        if (!finish)
        {
            
            if (myMath.check(plane.pos) == "null" && inventory.selectedIndex != -1)
            {
                if (inventory.getCardObject().mode == 0)
                {
                    if (inventory.getCardObject().checkNeeds(plane.pos))
                    {
                        GameObject go = Instantiate(inventory.getCardObject().gameObject);
                        if (connect != null)
                        {
                            connect.connectedObjects.Add(go.GetComponent<house>());
                        }
                        moveCount++;
                        inventory.useCard();
                        go.transform.parent = transform.Find("objects");
                        go.transform.localRotation = Quaternion.Euler(0, rot, 0);
                        go.transform.localScale = Vector3.one;
                        go.transform.localPosition = plane.pos;
                        go.GetComponent<house>().pos = plane.pos;
                        houses.Add(go.GetComponent<house>());
                        balance();
                        StartCoroutine(wait());
                    }
                    else
                    {
                        Debug.Log(plane.pos.ToString());
                    }
                }
                else
                {
                    bool checkNeed = false;
                    foreach (Vector3 v in inventory.getCardObject().extras)
                    {
                        checkNeed = checkNeed || myMath.checkRoads(plane.pos + myMath.rotxvec3(v, rot));                        
                    }
                    
                    if (checkNeed || inventory.getCardObject().checkNeeds(plane.pos))
                    {
                        foreach (Vector3 v in inventory.getCardObject().extras)
                        {
                            if (myMath.check(plane.pos + myMath.rotxvec3(v, rot)) != "null" || !inventory.getCardObject().checkNeeds(plane.pos + myMath.rotxvec3(v, rot), true))
                            {
                                return;
                            }
                        }
                        moveCount++;
                        List<house> hs = new List<house>();
                        foreach (Vector3 v in inventory.getCardObject().extras)
                        {
                            GameObject go1 = Instantiate(inventory.getCardObject().extraObject);
                            go1.transform.parent = transform.Find("objects");
                            if (myMath.getHouse(plane.pos + myMath.rotxvec3(v, rot) + new Vector3(0, -2, 0)) != null)
                            {
                                myMath.getHouse(plane.pos + myMath.rotxvec3(v, rot) + new Vector3(0, -2, 0)).connectedObjects.Add(go1.GetComponent<house>());
                            }
                            go1.transform.localRotation = Quaternion.Euler(0, rot, 0);
                            go1.transform.localScale = Vector3.one;
                            go1.transform.localPosition = plane.pos + myMath.rotxvec3(v, rot);
                            go1.GetComponent<house>().pos = plane.pos + myMath.rotxvec3(v, rot);
                            go1.GetComponent<house>().pop = inventory.getCardObject().pop;
                            go1.GetComponent<house>().typeName = inventory.getCardObject().typeName;
                            go1.GetComponent<house>().isHouse = inventory.getCardObject().isHouse;
                            go1.GetComponent<house>().weight = inventory.getCardObject().weight;
                            hs.Add(go1.GetComponent<house>());
                            houses.Add(go1.GetComponent<house>());
                        }
                        GameObject go = Instantiate(inventory.getCardObject().gameObject);
                        if (connect != null)
                        {
                            connect.connectedObjects.Add(go.GetComponent<house>());
                        }
                        inventory.useCard();
                        go.transform.parent = transform.Find("objects");
                        go.transform.localRotation = Quaternion.Euler(0, rot, 0);
                        go.transform.localScale = Vector3.one;
                        go.transform.localPosition = plane.pos;
                        go.GetComponent<house>().pos = plane.pos;
                        houses.Add(go.GetComponent<house>());
                        foreach (house h in hs)
                        {
                            h.connectedObjects.Add(go.GetComponent<house>());
                            go.GetComponent<house>().connectedObjects.Add(h);
                        }


                        balance();
                        StartCoroutine(wait());
                    }
                }
            }
        }
    }
    public void AddObject(plane plane, float rot, house connect, house obj)
    {
        if (!finish)
        {
            GameObject go = Instantiate(obj.gameObject);
            go.transform.parent = transform.Find("objects");
            go.transform.localRotation = Quaternion.Euler(0, rot, 0);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = plane.pos;
            go.GetComponent<house>().pos = plane.pos;
            houses.Add(go.GetComponent<house>());
            balance();
        }
    }
    public void fromStart(Vector3 pos)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Houses/yol"));
        go.transform.parent = transform.Find("objects");
        go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = pos;
        go.GetComponent<house>().pos = pos;
        houses.Add(go.GetComponent<house>());
        balance();
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        OnMove?.Invoke();
    }
    public void finalTheGame(Vector3 move)
    {
        finish = true;
        moveVector = move;
    }
}

