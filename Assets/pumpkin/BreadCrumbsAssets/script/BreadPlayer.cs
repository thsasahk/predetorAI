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
    /// <summary>
    /// 移動目標
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// 自身からtargetへのベクトル
    /// </summary>
    private Vector2 delta;
    /// <summary>
    /// GameDirecterオブジェクトのスクリプト
    /// </summary>
    public BreadDirecter directer;
    /// <summary>
    /// xyの移動方向と距離を記録
    /// </summary>
    private Vector2 direction;
    /// <summary>
    /// 目標への経路のx座標配列
    /// </summary>
    private float[] pathCol;
    /// <summary>
    /// 目標への経路のy座標配列
    /// </summary>
    private float[] pathRow;
    /// <summary>
    /// 配列の要素数
    /// </summary>
    private int element;
    /// <summary>
    /// 経路探索の配列に記録するベクトル
    /// </summary>
    private Vector2 path;
    /// <summary>
    /// 移動距離の少ない軸を移動するタイミングを決定
    /// </summary>
    private float fraction;

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
            SetPath(delta, directer.cellSize);
        }
        transform.position = Vector3.MoveTowards(current, nextCell, step * Time.deltaTime);//目標地点へ移動
    }

    private void SetPath(Vector2 d,Vector2 c)
    {
        if (d.x >= 0)
        {
            direction.x = directer.cellSize.x;//d.xが正なら正方向にマス目の長さ分移動
        }
        else
        {
            direction.x = -directer.cellSize.x;//d.xが負なら負方向にマス目の長さ分移動
        }
        if (d.y >= 0)
        {
            direction.y = directer.cellSize.y;//d.yが正なら正方向にマス目の長さ分移動
        }
        else
        {
            direction.y = -directer.cellSize.y;//d.yが負なら負方向にマス目の長さ分移動
        }
        path = current;//経路探索のスタート位置を記録
        if (d.x >= d.y)//x方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(d.x / c.x);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
        }
        else//y方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(d.y / c.y);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
        }
    }
}
