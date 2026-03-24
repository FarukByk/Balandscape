using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    Transform mCam;
    mainCode main;
    int maxPos,minPos = 5;
    public float zoom;
    float rotx, roty;
    float maxRot = 60;
    void Start()
    {
        mCam = Camera.main.transform;
        main = GameObject.Find("main").GetComponent<mainCode>();
        if (main != null)
        {
            maxPos = main.worldScale * 6;
        }
        else
        {
            maxPos = 20;
            maxRot = 45;
        }

        rotx = myMath.LoadFloat("camRotX",transform.localEulerAngles.x);
        roty = myMath.LoadFloat("camRotY", transform.localEulerAngles.x);
        mCam.transform.localPosition = new Vector3(mCam.transform.localPosition.x, 0, -zoom);
        transform.rotation = Quaternion.Euler(rotx, roty, 0);
        myMath.waitAndStart(2, () => { zoom = maxPos; });
    }

    
    void Update()
    {
        if (main != null)
        {
            if (Input.mouseScrollDelta.y < 0 && zoom < maxPos)
            {
                zoom += Time.deltaTime * 500;
            }
            else if (Input.mouseScrollDelta.y > 0 && zoom > minPos)
            {
                zoom -= Time.deltaTime * 500;
            }
        }
        mCam.transform.localPosition = Vector3.Lerp(mCam.transform.localPosition, new Vector3(mCam.transform.localPosition.x, 0, -zoom), Time.deltaTime * 10);

        rotx += Input.GetAxis("Vertical") * Time.deltaTime * 100;
        rotx = Mathf.Clamp(rotx, 10, maxRot);
        roty += -Input.GetAxis("Horizontal") * Time.deltaTime * 100;

        transform.rotation = Quaternion.Euler(rotx,roty,0);
    }
}
