using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadEnemy : MonoBehaviour
{
    /// <summary>
    /// Playerオブジェクトのposition
    /// </summary>
    private Vector2 playerPosition;
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
    /// 進行方向を-1～7の値で表す
    /// </summary>
    private int dirNumber = -1;
    /// <summary>
    /// 計算の結果を記録
    /// </summary>
    private Vector2 v;
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
    /// 進行方向に重みづけする
    /// </summary>
    private int[] priority;

    private int maxNumber = -1;

    void Start()
    {
        transform.position = startCell * directer.cellSize;
        nextCell = gameObject.transform.position;
        target = gameObject.transform.position;//初期化
        isStone = new bool[8];//隣接マスは常に8つ
        priority = new int[8];
    }

    void Update()
    {
        current = gameObject.transform.position;//変数を更新
        playerPosition = directer.player.transform.position;//変数を更新
        if (current != nextCell && analysis)//移動を開始したときに一度だけ行う処理
        {
            analysis = false;
            for (int n = 0; n < 8; n++)
            {
                isStone[n] = false;//初期化
            }
        }
        if (current == nextCell && analysis == false)//移動が完了しており、周辺を未探索の状態
        {
            analysis = true;
            for (int n = 0; n < directer.stoneNumber; n++)
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
        if (current == nextCell)
        {
            nextCell.x += Random.Range(-1, 2);
            nextCell.y += Random.Range(-1, 2);
            for(int n = 0 ; n < 8 ; n++)
            {
                if (priority[n] > maxNumber)
                {
                    maxNumber = priority[n];
                    switch (n)//進行方向と目的地へのベクトルを考慮してstoneオブジェクトを回避する
                    {
                        case 0:
                            nextCell.x -= 1;
                            nextCell.y += 1;
                            break;

                        case 1:
                            nextCell.y += 1;
                            break;

                        case 2:
                            nextCell.x += 1;
                            nextCell.y += 1;
                            break;

                        case 3:
                            nextCell.x -= 1;
                            break;

                        case 4:
                            nextCell.x += 1;
                            break;

                        case 5:
                            nextCell.x -= 1;
                            nextCell.y -= 1;
                            break;

                        case 6:
                            nextCell.y += 1;
                            break;

                        case 7:
                            nextCell.x += 1;
                            nextCell.y -= 1;
                            break;

                        default:
                            break;
                    }
                }
            }
            for (int n = directer.maxTrail - 1; n >= 0; n--)//directer.trailを走査
            {
                if (current.x - directer.trail[n].x <= 1 && current.x - directer.trail[n].x >= -1 &&
                    current.y - directer.trail[n].y <= 1 && current.y - directer.trail[n].y >= -1)
                    //自身に隣接するマスにplayerの足跡がある場合(新しい足跡から確認)
                {
                    nextCell = directer.trail[n];
                    for(int m = 0; m < 8; m++)
                    {
                        priority[m] = 0;//初期化
                    }
                    maxNumber = -1;//初期化
                    break;//見つけたらループから離脱
                }
            }
            nextCell.x = Mathf.Clamp(nextCell.x, -8, 8);
            nextCell.y = Mathf.Clamp(nextCell.y, -4, 4);
            v.x = current.x - nextCell.x;
            v.y = current.y - nextCell.y;
            if (v.x == 1 && v.y == -1)//左上
            {
                dirNumber = 0;
                priority[0] += 3;
                priority[1] += 2;
                priority[3] += 2;
                priority[2] += 1;
                priority[5] += 1;
                priority[7] -= 1;
            }
            if (v.x == 0 && v.y == -1)//真上
            {
                dirNumber = 1;
                priority[1] += 3;
                priority[0] += 2;
                priority[2] += 2;
                priority[3] += 1;
                priority[4] += 1;
                priority[6] -= 1;
            }
            if (v.x == -1 && v.y == -1)//右上
            {
                dirNumber = 2;
                priority[2] += 3;
                priority[1] += 2;
                priority[4] += 2;
                priority[0] += 1;
                priority[7] += 1;
                priority[5] -= 1;
            }
            if (v.x == 1 && v.y == 0)//左
            {
                dirNumber = 3;
                priority[3] += 3;
                priority[0] += 2;
                priority[5] += 2;
                priority[1] += 1;
                priority[6] += 1;
                priority[4] -= 1;
            }
            if (v.x == -1 && v.y == 0)//右
            {
                dirNumber = 4;
                priority[4] += 3;
                priority[2] += 2;
                priority[7] += 2;
                priority[1] += 1;
                priority[6] += 1;
                priority[3] -= 1;
            }
            if (v.x == 1 && v.y == 1)//左下
            {
                dirNumber = 5;
                priority[5] += 3;
                priority[3] += 2;
                priority[6] += 2;
                priority[0] += 1;
                priority[7] += 1;
                priority[2] -= 1;
            }
            if (v.x == 0 && v.y == 1)//真下
            {
                dirNumber = 6;
                priority[6] += 3;
                priority[5] += 2;
                priority[7] += 2;
                priority[3] += 1;
                priority[4] += 1;
                priority[1] -= 1;
            }
            if (v.x == -1 && v.y == 1)//右下
            {
                dirNumber = 7;
                priority[7] += 3;
                priority[6] += 2;
                priority[4] += 2;
                priority[2] += 1;
                priority[5] += 1;
                priority[0] -= 1;
            }
            if (v.x == 0 && v.y == 0)//移動しない
            {
                dirNumber = -1;
            }
        }
        if (dirNumber >= 0)//停止状態ではない
        {
            if (isStone[dirNumber])//自身の進行方向にstoneオブジェクトが存在する場合
            {
                switch (dirNumber)//進行方向と目的地へのベクトルを考慮してstoneオブジェクトを回避する
                {
                    case 0:
                        priority[0] = -1;
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
                        priority[1] = -1;
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
                        priority[2] = -1;
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
                        priority[3] = -1;
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
                        priority[4] = -1;
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
                        priority[5] = -1;
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
                        priority[6] = -1;
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
                        priority[7] = -1;
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
            }
        }
        transform.position = Vector3.MoveTowards(current, nextCell, step * Time.deltaTime);//目標地点へ移動
    }

    private void SetPath(Vector2 d, Vector2 c)
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
