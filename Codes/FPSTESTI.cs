using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSTESTI : MonoBehaviour
{
    float fps = 200;
    void Update()
    {
        fps = Mathf.Lerp(fps, (1 / Time.deltaTime), Time.deltaTime/2);
        GetComponent<TMP_Text>().text = ((int)fps).ToString() + " FPS";
    }
    
}
