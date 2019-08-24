using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadPlayer : MonoBehaviour
{
    /// <summary>
    /// タッチ情報を格納
    /// </summary>
    public Vector2 touch;
    /// <summary>
    /// 現在位置を記録
    /// </summary>
    private Vector2 current;
    /// <summary>
    /// 移動する位置
    /// </summary>
    private Vector2 nextCell;
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float step;

    void Start()
    {
        nextCell = gameObject.transform.position;//初期化
    }

    void Update()
    {
        current = gameObject.transform.position;//変数を更新
        nextCell = touch;//仮の記述
        transform.position = Vector3.MoveTowards(current, nextCell, step * Time.deltaTime);//目標地点へ移動
    }
}
