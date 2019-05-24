using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eye_flocks : MonoBehaviour
{
    /// <summary>
    /// 自身のpositionを記録
    /// </summary>
    private Vector2 position;
    /// <summary>
    /// 自身の方向ベクトル
    /// </summary>
    public Vector2 direction;
    /// <summary>
    /// 視界の長さ
    /// </summary>
    public float viewLength;
    /// <summary>
    /// 視界の角度
    /// </summary>
    public float viewAngle;
    /// <summary>
    /// 視界内の仲間
    /// </summary>
    public GameObject[] member;
    /// <summary>
    /// オブジェクトの角度
    /// </summary>
    private float angle;
    /// <summary>
    /// member配列に格納されているオブジェクトの数
    /// </summary>
    public int number = 0;

    void Start()
    {

    }

    void Update()
    {
        angle = transform.eulerAngles.z * (Mathf.PI / 180.0f);//自身の向いている方向角度をラジアン化、参考元→http://ftvoid.com/blog/post/631
        direction.x = Mathf.Cos(angle);//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(angle);//自身の方向ベクトルを取得
    }
}
