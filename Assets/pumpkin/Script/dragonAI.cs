using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonAI : MonoBehaviour
{
    /// <summary>
    /// playerオブジェクトの現在位置、GameDirecterから取得
    /// </summary>
    public Vector2 pPosition;
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float step;
    /// <summary>
    /// 自身の現在位置
    /// </summary>
    private Vector2 current;
    /// <summary>
    /// 目標地点
    /// </summary>
    private Vector2 target;

    void Start()
    {
        target = gameObject.transform.position;//初期のtargetを現在位置に設定
    }

    void Update()
    {
        current = gameObject.transform.position;//現在位置をcurrentに記録
        if (current == target)
        {
            SetTarget(pPosition - current);//pPositionとcurrentをもとに目標地点を設定
        }
        transform.position = Vector3.MoveTowards(current, target, step * Time.deltaTime);//目標地点へ移動
    }

    /// <summary>
    /// 目標地点を設定する
    /// </summary>
    /// <param name="t">pPosition - currentで決定</param>
    /// <returns></returns>
    private Vector2 SetTarget(Vector2 t)
    {
        if (t.x != 0)
        {
            target.x += Mathf.Sign(t.x);
        }
        if (t.y != 0)
        {
            target.y += Mathf.Sign(t.y);
        }
        return target;
    }
}
