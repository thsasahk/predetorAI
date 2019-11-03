using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlayer : MonoBehaviour
{
    /// <summary>
    /// マス目のサイズ
    /// </summary>
    public Vector2 ratio;
    /// <summary>
    /// 自身の座標
    /// </summary>
    private Vector2 playerPos;
    /// <summary>
    /// 移動先の座標
    /// </summary>
    private Vector2 nextCell;
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float speed;
    /// <summary>
    /// 2次元配列情報
    /// </summary>
    public Layer2D layer;
    /// <summary>
    /// 通路
    /// </summary>
    private int chipNone = 0;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().localScale =
            gameObject.GetComponent<RectTransform>().localScale * ratio;//マス目のサイズに合わせて自身のサイズを変更する
        playerPos = transform.position;
        nextCell = transform.position;
    }

    void Update()
    {
        playerPos = transform.position;//自身の座標を記録
        if (playerPos == nextCell)
        {
            //目標地点を設定、テスト用ビルド時コメントアウト
            SetNext(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        //目標地点へ移動
        transform.position = Vector3.MoveTowards(playerPos, nextCell, speed * Time.deltaTime);
    }

    /// <summary>
    /// 上下左右の入力情報を符号化して目標地点を決定
    /// </summary>
    /// <param name="h">左右の入力情報</param>
    /// <param name="v">上下の入力情報</param>
    /// <returns></returns>
    private void SetNext(float h, float v)
    {
        if (h != 0)
        {
            nextCell.x = playerPos.x + Mathf.Sign(h) * ratio.x;
        }
        if (v != 0)
        {
            nextCell.y = playerPos.y + Mathf.Sign(v) * ratio.y;
        }
        if (layer.Get(Mathf.RoundToInt((nextCell.x - ratio.x / 2) / ratio.x), 
            Mathf.RoundToInt((nextCell.y - ratio.y / 2) / ratio.y)) != chipNone)
        {
            nextCell = playerPos;
        }
    }
}
