﻿using System.Collections;
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
    /// <summary>
    /// 移動目標
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// 自身からtargetへのベクトル
    /// </summary>
    private Vector2 delta;

    void Start()
    {
        nextCell = gameObject.transform.position;//初期化
        target = gameObject.transform.position;//初期化
    }

    void Update()
    {
        current = gameObject.transform.position;//変数を更新
        nextCell = touch;//仮の記述
        if (target != touch && current == nextCell)
        {
            target = touch;//移動目標を更新
            delta = target - current;
            SetPath(delta);
        }
        transform.position = Vector3.MoveTowards(current, nextCell, step * Time.deltaTime);//目標地点へ移動
    }

    private void SetPath(Vector2 d)
    {

    }
}
