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
    /// <summary>
    /// 最も重い重みを記録
    /// </summary>
    private int maxNumber = -1;

    void Start()
    {
        transform.position = startCell * directer.cellSize;
        nextCell = gameObject.transform.position;
        target = gameObject.transform.position;//初期化
        isStone = new bool[8];//隣接マスは常に8つ
        priority = new int[8];
        dirNumber = Random.Range(0, 8);//最初の進行方向をランダムに決定
        priority[dirNumber] = 1;
    }

    void Update()
    {
        current = gameObject.transform.position;//変数を更新
        playerPosition = directer.player.transform.position;//変数を更新
        Search(current - nextCell);
        if (current == nextCell)
        {
            SetNext();
            Chase();
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
    /// 自身の進行方向の障害物の有無を確認し、あるならnextCellの値を変更する
    /// </summary>
    /// <param name="n">dirNumber</param>
    private void Avoid(int n)
    {
        if (isStone[n])//自身の進行方向にstoneオブジェクトが存在する場合
        {
            delta = nextCell - current;
            absDelX = Mathf.Abs(delta.x);
            absDelY = Mathf.Abs(delta.y);
            switch (n)//進行方向と目的地へのベクトルを考慮してstoneオブジェクトを回避する
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

    /// <summary>
    /// 自身の進行方向を記録し、重みを計算する
    /// </summary>
    /// <param name="v">current - nextCell</param>
    private void SetDir(Vector2 v)
    {
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
        if (current.x == -8)
        {
            priority[0] = -1;
            priority[3] = -1;
            priority[5] = -1;
        }
        if (current.x == 8)
        {
            priority[2] = -1;
            priority[4] = -1;
            priority[7] = -1;
        }
        if (current.y == -4)
        {
            priority[5] = -1;
            priority[6] = -1;
            priority[7] = -1;
        }
        if (current.y == 4)
        {
            priority[0] = -1;
            priority[1] = -1;
            priority[2] = -1;
        }
        maxNumber = -1;//初期化
    }

    /// <summary>
    /// 自身の周囲のマスの障害物の有無を調査し、記録する
    /// </summary>
    /// <param name="v">current - nextCell</param>
    private void Search(Vector2 v)
    {
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
    }

    /// <summary>
    /// 方角の重みを参考にして自身の移動先を決定する
    /// </summary>
    /// <param name="n">繰り返しの処理に使う変数</param>
    private void SetNext(int n = 0)
    {
        for (n = 0; n < 8; n++)
        {
            if (priority[n] > maxNumber)
            {
                maxNumber = priority[n];
                switch (n)//最も大きいpriorityを持つ方向へとnextCellを設定する
                {
                    case 0:
                        nextCell.x = current.x - 1;
                        nextCell.y = current.y + 1;
                        break;

                    case 1:
                        nextCell.y = current.y + 1;
                        break;

                    case 2:
                        nextCell.x = current.x + 1;
                        nextCell.y = current.y + 1;
                        break;

                    case 3:
                        nextCell.x = current.x - 1;
                        break;

                    case 4:
                        nextCell.x = current.x + 1;
                        break;

                    case 5:
                        nextCell.x = current.x - 1;
                        nextCell.y = current.y - 1;
                        break;

                    case 6:
                        nextCell.y = current.y - 1;
                        break;

                    case 7:
                        nextCell.x = current.x + 1;
                        nextCell.y = current.y - 1;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// playerの足跡を探し、発見したら追跡
    /// </summary>
    /// <param name="n">繰り返しの処理に使う変数</param>
    /// <param name="m">繰り返しの処理に使う変数</param>
    private void Chase(int n = 0,int m = 0)
    {
        for (n = directer.maxTrail - 1; n >= 0; n--)//directer.trailを走査
        {
            if (current.x - directer.trail[n].x <= 1 && current.x - directer.trail[n].x >= -1 &&
                current.y - directer.trail[n].y <= 1 && current.y - directer.trail[n].y >= -1)
            //自身に隣接するマスにplayerの足跡がある場合(新しい足跡から確認)
            {
                nextCell = directer.trail[n];
                for (m = 0; m < 8; m++)
                {
                    priority[m] = 0;//初期化
                }
                break;//見つけたらループから離脱
            }
        }
    }
}
