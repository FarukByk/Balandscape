using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiPoses : MonoBehaviour
{
    RectTransform rect;
    Vector2 baseRectPos;
    public Transform baseTransform;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        baseRectPos = rect.localPosition;
        
    }
    void Update()
    {
        rect.position = (Vector2)Camera.main.WorldToScreenPoint(baseTransform.position) + baseRectPos;
    }
}
