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
    //private int n;
    private int pathNumber;
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
    private bool analysis = false;
    /// <summary>
    /// stoneオブジェクトの位置を受け取る配列
    /// </summary>
    public Vector2[] stonePos;
    /// <summary>
    /// 進行方向を-1～7の値で表す
    /// </summary>
    private int dirNumber = -1;

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
        if (current == nextCell && current != directer.trail[directer.maxTrail - 1])
            //移動が終了していて、最新の足跡と現在地が異なるとき
        {
            SetTrail(directer.maxTrail - 1);
        }
        Search(Vector2.zero);
        if (current == nextCell && element > 0)//自身の移動が終了しており、移動経路配列の要素数が0でないタイミング
        {
            nextCell.x = pathCol[pathNumber];
            nextCell.y = pathRow[pathNumber];
            if (pathNumber < element - 1) //nを配列の要素数以上にしない
            {
                pathNumber++;
            }
            nextCell.x = Mathf.Clamp(nextCell.x, -8, 8);
            nextCell.y = Mathf.Clamp(nextCell.y, -4, 4);
            SetDir(current - nextCell);
        }
        if (dirNumber >= 0)//停止状態ではない
        {
            Avoid(dirNumber);
        }
        transform.position = Vector3.MoveTowards(current, nextCell, step * Time.deltaTime);//目標地点へ移動
    }

    /// <summary>
    /// 経路探索を行う
    /// </summary>
    /// <param name="d">delta</param>
    /// <param name="c">directer.cellSize</param>
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
            for (pathNumber = 0; pathNumber <= element - 1; pathNumber++)
            {
                path.x += direction.x;//次のマスのx座標を計算
                pathCol[pathNumber] = path.x;//配列に記録
                if (fraction >= 0)
                {
                    path.y += direction.y;//次のマスのy座標を計算
                    fraction -= absDelX;
                }
                fraction += absDelY;
                pathRow[pathNumber] = path.y;
            }
        }
        else//y方向への移動距離が長い場合
        {
            element = Mathf.CeilToInt(absDelY / c.y);//移動するマス目の数を配列の要素数とする
            pathCol = new float[element];
            pathRow = new float[element];
            fraction = absDelX * 2 - absDelY;//x方向への移動距離とy方向への移動距離の比率で移動タイミングを決定
            for (pathNumber = 0; pathNumber <= element - 1; pathNumber++)
            {
                path.y += direction.y;//次のマスのx座標を計算
                pathRow[pathNumber] = path.y;//配列に記録
                if (fraction >= 0)
                {
                    path.x += direction.x;//次のマスのy座標を計算
                    fraction -= absDelY;
                }
                fraction += absDelX;
                pathCol[pathNumber] = path.x;
            }
        }
        pathNumber = 0;//初期化
    }

    /// <summary>
    /// 自身の進行方向の障害物の有無を確認し、あるならnextCellの値を変更する
    /// </summary>
    /// <param name="n">dirNumber</param>
    private void Avoid(int n)
    {
        if (isStone[n])//自身の進行方向にstoneオブジェクトが存在する場合
        {
            switch (n)//進行方向と目的地へのベクトルを考慮してstoneオブジェクトを回避する
            {
                case 0:
                    if (absDelX >= absDelY)
                    {
                        nextCell.y--;
                    }
                    else
                    {
                        nextCell.x++;
                    }
                    break;

                case 1:
                    if (delta.x >= 0)
                    {
                        nextCell.x++;
                    }
                    else
                    {
                        nextCell.x--;
                    }
                    break;

                case 2:
                    if (absDelX >= absDelY)
                    {
                        nextCell.y--;
                    }
                    else
                    {
                        nextCell.x--;
                    }
                    break;

                case 3:
                    if (delta.y >= 0)
                    {
                        nextCell.y++;
                    }
                    else
                    {
                        nextCell.y--;
                    }
                    break;

                case 4:
                    if (delta.y >= 0)
                    {
                        nextCell.y++;
                    }
                    else
                    {
                        nextCell.y--;
                    }
                    break;

                case 5:
                    if (absDelX >= absDelY)
                    {
                        nextCell.y++;
                    }
                    else
                    {
                        nextCell.x++;
                    }
                    break;

                case 6:
                    if (delta.x >= 0)
                    {
                        nextCell.x++;
                    }
                    else
                    {
                        nextCell.x--;
                    }
                    break;

                case 7:
                    if (absDelX >= absDelY)
                    {
                        nextCell.y++;
                    }
                    else
                    {
                        nextCell.x--;
                    }
                    break;

                default:
                    break;
            }
            target = nextCell;//回避先から経路を探索をやり直す
        }
    }

    /// <summary>
    /// 自身の周囲のマスの障害物の有無を調査し、記録する
    /// </summary>
    /// <param name="v">計算結果を納める変数</param>
    /// <param name="n">繰り返しの処理に使う変数</param>
    /// <param name="m">繰り返しの処理に使う変数</param>
    private void Search(Vector2 v,int n = 0,int m = 0)
    {
        if (current != nextCell && analysis)//移動を開始したときに一度だけ行う処理
        {
            analysis = false;
            for (n = 0; n < 8; n++)
            {
                isStone[n] = false;//初期化
            }
        }
        if (current == nextCell && analysis == false)//移動が完了しており、周辺を未探索の状態
        {
            analysis = true;
            for (m = 0; m < directer.stoneNumber; m++)
            {
                v.x = current.x - stonePos[m].x;
                v.y = current.y - stonePos[m].y;
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
    }

    /// <summary>
    /// 自身の進行方向を記録
    /// </summary>
    /// <param name="v">current - nextCell</param>
    private void SetDir(Vector2 v)
    {
        if (v.x == 1 && v.y == -1)//左上
        {
            dirNumber = 0;
        }
        if (v.x == 0 && v.y == -1)//真上
        {
            dirNumber = 1;
        }
        if (v.x == -1 && v.y == -1)//右上
        {
            dirNumber = 2;
        }
        if (v.x == 1 && v.y == 0)//左
        {
            dirNumber = 3;
        }
        if (v.x == -1 && v.y == 0)//右
        {
            dirNumber = 4;
        }
        if (v.x == 1 && v.y == 1)//左下
        {
            dirNumber = 5;
        }
        if (v.x == 0 && v.y == 1)//真下
        {
            dirNumber = 6;
        }
        if (v.x == -1 && v.y == 1)//右下
        {
            dirNumber = 7;
        }
        if (v.x == 0 && v.y == 0)//移動しない
        {
            dirNumber = -1;
        }
    }

    /// <summary>
    /// directer.trailに自身の足跡を記録
    /// </summary>
    /// <param name="t">directer.maxTrail - 1</param>
    /// <param name="n">繰り返しの処理に使う変数</param>
    private void SetTrail(int t,int n = 0)
    {
        for (n = 0; n < t; n++)//足跡を一つずらして最も古いものを削除
        {
            directer.trail[n] = directer.trail[n + 1];
        }
        directer.trail[t] = current;//最新の足跡を更新
    }
}
