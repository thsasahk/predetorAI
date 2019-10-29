using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlayer : MonoBehaviour
{
    /// <summary>
    /// マス目のサイズ
    /// </summary>
    public Vector2 ratio;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x * ratio.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y * ratio.y);//マス目のサイズに合わせて自身のサイズを変更する
    }

    void Update()
    {
        
    }
}
