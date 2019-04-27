using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    /// <summary>
    /// マス目のサイズ
    /// </summary>
    [SerializeField] private float cellSize;
    /// <summary>
    /// 現在地点
    /// </summary>
    private Vector2 current;
    /// <summary>
    /// 目標地点
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float step;

    void Start()
    {
        target = gameObject.transform.position;//目標地点を現在地点に修正
    }

    void Update()
    {
        current = gameObject.transform.position;//現在地点を記録
        if (target == current)
        {
            SetTarget(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));//移動終了してから新たな目標地点を設定
        }
        transform.position = Vector3.MoveTowards(current, target, step * Time.deltaTime);//目標地点へ移動
    }

    /// <summary>
    /// 上下左右の入力情報を符号化して目標地点を決定
    /// </summary>
    /// <param name="h">左右の入力情報</param>
    /// <param name="v">上下の入力情報</param>
    /// <returns></returns>
    private Vector2 SetTarget(float h,float v)
    {
        if (h != 0)
        {
            target.x += Mathf.Sign(h) * cellSize;
        }
        if (v != 0)
        {
            target.y += Mathf.Sign(v) * cellSize;
        }
        return target;
    }
}
