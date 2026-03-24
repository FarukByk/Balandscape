using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseControl : MonoBehaviour
{
    target target;
    Transform hammer;
    GameObject hedef;
    public GameObject prefab;
    public LayerMask planeLayer;
    mainCode main;
    bool delete;
    RaycastHit hit;
    Inventory inv;
    private void Start()
    {
        inv = FindAnyObjectByType<Inventory>(); 
        target = FindAnyObjectByType<target>();
        hedef = target.gameObject;
        hammer = target.transform.Find("hammer");
        
        main = FindAnyObjectByType<mainCode>();
        hammer.parent = main.transform;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown("q"))
        {
            delete = !delete;
            target.delete = delete;
        }
        if (Physics.Raycast(ray, out hit, 200, planeLayer) && prefab != null && !inv.open)
        {
            target.active = true;
            hedef.transform.position = hit.transform.position;
            hedef.transform.rotation = main.transform.rotation;
            if (Input.GetMouseButtonDown(0) && !delete)
            {
                main.AddObject(hit.collider.gameObject.GetComponent<plane>(), target.rot, hit.collider.gameObject.GetComponent<plane>().connectHouse);
            }
            else if (delete && Input.GetMouseButtonDown(0) && myMath.checkColumn(hit.collider.gameObject.GetComponent<plane>().pos) && main.inventory.gold >= 5)
            {
                hammer.GetComponent<Animator>().SetTrigger("hit");
                hammer.transform.position = hit.collider.gameObject.transform.position;
                main.inventory.addGold(-5);
                float zRot = Mathf.Atan2(target.transform.localPosition.x, target.transform.localPosition.z) * Mathf.Rad2Deg;
                hammer.localRotation = Quaternion.Euler(0, zRot, 0);
                StartCoroutine(waitHammer(hit.collider.gameObject.GetComponent<plane>()));
                delete = false;
                target.delete = delete;
            }
            
        }
        else
        {
            target.active = false;
        }
        if (delete && Input.GetMouseButtonDown(0) && main.inventory.gold < 5 && !inv.open)
        {
            delete = false;
            target.delete = delete;
        }
    }


    IEnumerator waitHammer(plane plane)
    {
        yield return new WaitForSeconds(0.25f);
        main.DeleteObject(plane);
    }
}
