using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public Vector2 ratio;
    void Start()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x * ratio.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y * ratio.y);
    }

    void Update()
    {
        
    }
}
