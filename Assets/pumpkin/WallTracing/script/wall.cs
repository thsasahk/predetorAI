using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    /// <summary>
    /// マス目のサイズ
    /// </summary>
    public Vector2 ratio;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().localScale = 
            gameObject.GetComponent<RectTransform>().localScale * ratio;//マス目のサイズに合わせて自身のサイズを変更する
    }

    void Update()
    {
        
    }
}
