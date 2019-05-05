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
    /// <summary>
    /// 次に移動するマス
    /// </summary>
    private Vector2 nextCell;
    /// <summary>
    /// 目標への経路のx座標配列
    /// </summary>
    private float[] pathCol;
    /// <summary>
    /// 目標への経路のy座標配列
    /// </summary>
    private float[] pathRow;
    /// <summary>
    /// マス目のサイズ
    /// </summary>
    [SerializeField] private Vector2 cellSize;
    /// <summary>
    /// 配列の要素番号
    /// </summary>
    private int num;
    /// <summary>
    /// 配列の最終要素の番号
    /// </summary>
    private int maxNum;
    /// <summary>
    /// 配列に記録するx座標
    /// </summary>
    private float col;
    /// <summary>
    /// 配列に記録するy座標
    /// </summary>
    private float row;
    /// <summary>
    /// オブジェクトとターゲットのx座標の差
    /// </summary>
    private float deltaCol;
    /// <summary>
    /// オブジェクトとターゲットのy座標の差
    /// </summary>
    private float deltaRow;
    /// <summary>
    /// x移動の方向と距離
    /// </summary>
    private float stepCol;
    /// <summary>
    /// y移動の方向と距離
    /// </summary>
    private float stepRow;
    /// <summary>
    /// 配列の要素数
    /// </summary>
    private int element;
    /// <summary>
    /// 移動距離の少ない軸を移動するタイミングを決定
    /// </summary>
    private float fraction;
    /// <summary>
    /// オブジェクトからターゲットへの距離
    /// </summary>
    private Vector2 delta;
    /// <summary>
    /// 索敵範囲
    /// </summary>
    private float searchEria;

    void Start()
    {
        target = gameObject.transform.position;//ターゲットを現在位置に設定
        nextCell = target;//最初の移動先を現在位置に設定
        pathCol = new float[1] { target.x };//最初の移動経路に現在位置を設定
        pathRow = new float[1] { target.y };//最初の移動経路に現在位置を設定
    }

    void Update()
    {
        current = gameObject.transform.position;//現在位置をcurrentに記録
        delta = pPosition - current;
        if (target != pPosition && current == nextCell && //目標が移動しており、自身の移動が終了しているタイミング
            Mathf.RoundToInt(Mathf.Abs(delta.x) + Mathf.Abs(delta.y)) <= searchEria) //索敵範囲内
        {
            SetDirection(delta);
            SetPath(delta);
        }
        if (current == nextCell && element > 0)//自身の移動が終了しており、移動経路配列の要素数が0でないタイミング
        {
            SetNextCell(num);
            if (num < maxNum && //numを配列の要素数以上にしない
                Mathf.RoundToInt(Mathf.Abs(delta.x) + Mathf.Abs(delta.y)) <= searchEria)//索敵範囲外なら現在値で止まる
            {
                num++;
            }
        }
        transform.position = Vector3.MoveTowards(current, nextCell, step * Time.deltaTime);//現在位置からnextCellへ定速で移動
    }

    /// <summary>
    /// ターゲットへの経路探索
    /// </summary>
    /// <param name="d">pPosition - current</param>
    private void SetPath(Vector2 d)
    {
        target = pPosition;//目標の位置を記録
        col = current.x;//スタートのx座標を記録
        row = current.y;//スタートのy座標を記録
        if (deltaCol >= deltaRow)//x方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(deltaCol / cellSize.x);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
            maxNum = element - 1;//配列の最終要素の番号を記録
            fraction = deltaRow * 2 - deltaCol;//x方向への移動距離とy方向への移動距離の比率で移動タイミングを決定
            for (num = 0; num <= maxNum; num++)
            {
                col += stepCol;//次のマスのx座標を計算
                pathCol[num] = col;//配列に記録
                if (fraction >= 0)//
                {
                    row += stepRow;//次のマスのy座標を計算
                    fraction -= deltaCol;
                }
                fraction += deltaRow;
                pathRow[num] = row;
            }
        }
        else//y方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(deltaRow / cellSize.y);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
            maxNum = element - 1;//配列の最終要素の番号を記録
            fraction = deltaCol * 2 - deltaRow;//x方向への移動距離とy方向への移動距離の比率で移動タイミングを決定
            for (num = 0; num <= maxNum; num++)
            {
                row += stepRow;//次のマスのy座標を計算
                pathRow[num] = row;//配列に記録
                if (fraction >= 0)
                {
                    col += stepCol;//次のマスのx座標を計算
                    fraction -= deltaRow;
                }
                fraction += deltaCol;
                pathCol[num] = col;
            }
        }
        num = 0;//要素番号を初期化
    }

    /// <summary>
    /// 次の移動先を決定
    /// </summary>
    /// <param name="i">num</param>
    private void SetNextCell(int i)
    {
        nextCell.x = pathCol[i];
        nextCell.y = pathRow[i];
    }

    /// <summary>
    /// 移動方向を計算
    /// </summary>
    /// <param name="d">pPosition - current</param>
    private void SetDirection(Vector2 d)
    {
        if (d.x >= 0)
        {
            stepCol = cellSize.x;//d.xが正なら正方向にマス目の長さ分移動
        }
        else
        {
            stepCol = -cellSize.x;//d.xが負なら負方向にマス目の長さ分移動
        }
        if (d.y >= 0)
        {
            stepRow = cellSize.y;//d.yが正なら正方向にマス目の長さ分移動
        }
        else
        {
            stepRow = -cellSize.y;//d.yが負なら負方向にマス目の長さ分移動
        }
        deltaCol = Mathf.Abs(d.x);//x方向の移動距離を格納
        deltaRow = Mathf.Abs(d.y);//y方向の移動距離を格納
    }
}
