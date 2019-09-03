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
    /// <summary>
    /// 繰り返し処理に使う変数
    /// </summary>
    private int n;
    /// <summary>
    /// スタート時のマス
    /// </summary>
    public Vector2 startCell;
    /// <summary>
    /// delta.xの絶対値を格納
    /// </summary>
    private float absDelX;
    /// <summary>
    /// delta.yの絶対値を格納
    /// </summary>
    private float absDelY;
    /// <summary>
    /// 自身の隣接マスのstoneオブジェクトの有無を記録
    /// </summary>
    private bool[] isStone;
    /// <summary>
    /// 隣接マスの分析が完了していることを確認
    /// </summary>
    private bool analysis;
    /// <summary>
    /// stoneオブジェクトの位置を受け取る配列
    /// </summary>
    public Vector2[] stonePos;
    /// <summary>
    /// current - stonePosの計算結果を記録
    /// </summary>
    private Vector2 v;

    void Start()
    {
        isStone = new bool[8];//隣接マスは常に8つ
        transform.position = startCell * directer.cellSize;
        nextCell = gameObject.transform.position;//初期化
        target = gameObject.transform.position;//初期化
        touch = gameObject.transform.position;//初期化
        for(int i = 0; i < directer.maxTrail; i++)//配列の初期値を設定
        {
            directer.trail[i] = startCell;
        }
    }

    void Update()
    {
        current = gameObject.transform.position;//変数を更新
        if (target != touch && current == nextCell)
        {
            target = touch;//移動目標を更新
            delta = target - current;
            SetPath(delta, directer.cellSize);
        }
        if (current == nextCell && analysis == false)//移動が完了しており、周辺を未探索の状態
        {
            analysis = true;
            for(int n = 0; n < directer.stoneNumber; n++)
            {
                v.x = current.x - stonePos[n].x;
                v.y = current.y - stonePos[n].y;
                if (v.x == 1 && v.y == -1)//左上
                {
                    isStone[0] = true;
                }
                if (v.x == 0 && v.y == -1)//真上
                {
                    isStone[1] = true;
                }
                if (v.x == -1 && v.y == -1)//右上
                {
                    isStone[2] = true;
                }
                if (v.x == 1 && v.y == 0)//左
                {
                    isStone[3] = true;
                }
                if (v.x == -1 && v.y == 0)//右
                {
                    isStone[4] = true;
                }
                if (v.x == 1 && v.y == 1)//左下
                {
                    isStone[5] = true;
                }
                if (v.x == 0 && v.y == 1)//真下
                {
                    isStone[6] = true;
                }
                if (v.x == -1 && v.y == 1)//右下
                {
                    isStone[7] = true;
                }
            }
        }
        else
        {
            analysis = false;
            for(int n = 0; n < 8; n++)
            {
                isStone[n] = false;//初期化
            }
        }
        if (current == nextCell && current != directer.trail[directer.maxTrail - 1])
            //移動が終了していて、最新の足跡と現在地が異なるとき
        {
            for(int i = 0; i < directer.maxTrail - 1; i++)//足跡を一つずらして最も古いものを削除
            {
                directer.trail[i] = directer.trail[i + 1];
            }
            directer.trail[directer.maxTrail - 1] = current;//最新の足跡を更新
        }
        if (current == nextCell && element > 0)//自身の移動が終了しており、移動経路配列の要素数が0でないタイミング
        {
            nextCell.x = pathCol[n];
            nextCell.y = pathRow[n];
            if (n < element - 1) //nを配列の要素数以上にしない
            {
                n++;
            }
        }
        nextCell.x = Mathf.Clamp(nextCell.x, -8, 8);
        nextCell.y = Mathf.Clamp(nextCell.y, -4, 4);
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
        absDelX = Mathf.Abs(d.x);
        absDelY = Mathf.Abs(d.y);
        path = current;//経路探索のスタート位置を記録
        if (absDelX >= absDelY)//x方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(absDelX / c.x);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
            fraction = absDelY * 2 - absDelX;//x方向への移動距離とy方向への移動距離の比率で移動タイミングを決定
            for (n = 0; n <= element - 1; n++)
            {
                path.x += direction.x;//次のマスのx座標を計算
                pathCol[n] = path.x;//配列に記録
                if (fraction >= 0)
                {
                    path.y += direction.y;//次のマスのy座標を計算
                    fraction -= absDelX;
                }
                fraction += absDelY;
                pathRow[n] = path.y;
            }
        }
        else//y方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(absDelY / c.y);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
            fraction = absDelX * 2 - absDelY;//x方向への移動距離とy方向への移動距離の比率で移動タイミングを決定
            for (n = 0; n <= element - 1; n++)
            {
                path.y += direction.y;//次のマスのx座標を計算
                pathRow[n] = path.y;//配列に記録
                if (fraction >= 0)
                {
                    path.x += direction.x;//次のマスのy座標を計算
                    fraction -= absDelY;
                }
                fraction += absDelX;
                pathCol[n] = path.x;
            }
        }
        n = 0;//初期化
    }
}
