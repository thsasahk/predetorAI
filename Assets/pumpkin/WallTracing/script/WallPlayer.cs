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
        gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x * ratio.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y * ratio.y);//マス目のサイズに合わせて自身のサイズを変更する
        playerPos = transform.position;
        nextCell = transform.position;
    }

    void Update()
    {
        playerPos = transform.position;//自身の座標を記録
        if (playerPos == nextCell)
        {
            //目標地点へ移動、テスト用ビルド時コメントアウト
            transform.position = Vector3.MoveTowards(playerPos, 
                SetNext(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")), speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 上下左右の入力情報を符号化して目標地点を決定
    /// </summary>
    /// <param name="h">左右の入力情報</param>
    /// <param name="v">上下の入力情報</param>
    /// <returns></returns>
    private Vector2 SetNext(float h, float v)
    {
        if (h != 0)
        {
            nextCell.x = playerPos.x + Mathf.Sign(h) * ratio.x;
        }
        if (v != 0)
        {
            nextCell.y = playerPos.y + Mathf.Sign(v) * ratio.y;
        }
        if (layer.Get(Mathf.RoundToInt(nextCell.x / ratio.x - nextCell.x / 2), 
            Mathf.RoundToInt(nextCell.y / ratio.y - nextCell.y / 2)) == chipNone)
        {
            return nextCell;
        }
        else
        {
            return playerPos;
        }
    }
}
