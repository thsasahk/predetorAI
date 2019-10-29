using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlayer : MonoBehaviour
{
    /// <summary>
    /// マス目のサイズ
    /// </summary>
    public Vector2 ratio;
    /// <summary>
    /// 自身の座標
    /// </summary>
    private Vector2 playerPos;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x * ratio.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y * ratio.y);//マス目のサイズに合わせて自身のサイズを変更する
    }

    void Update()
    {
        playerPos = transform.position;//自身の座標を記録
    }
}
