using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonAI : MonoBehaviour
{
    /// <summary>
    /// GameDirecterオブジェクトのスクリプト
    /// </summary>
    private GameDirector gdController;
    /// <summary>
    /// 移動方向と速度を表すベクトル
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// X方向の移動速度
    /// </summary>
    [SerializeField] private float speedX;
    /// <summary>
    /// Y方向の移動速度
    /// </summary>
    [SerializeField] private float speedY;
    /// <summary>
    /// playerオブジェクトの現在位置
    /// </summary>
    public Vector2 pPosition;
    /// <summary>
    /// 自身の現在位置
    /// </summary>
    private Vector2 dPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dPosition = gameObject.transform.position;//自身の現在位置をdPositionに記録
        gameObject.transform.Translate(Chase(pPosition, dPosition));//playerオブジェクトを目標に移動
    }

    /// <summary>
    /// 自身の位置とplayerオブジェクトの位置を参照して移動方向を決定
    /// </summary>
    /// <param name="p">pPosition</param>
    /// <param name="d">dPosition</param>
    /// <returns></returns>
    private Vector2 Chase(Vector2 p, Vector2 d)
    {
        if (d.x > p.x)
        {
            speed.x = -speedX;
        }
        else if (d.x < p.x)
        {
            speed.x = speedX;
        }

        if (d.y > p.y)
        {
            speed.y = -speedY;
        }
        else if (d.y < p.y)
        {
            speed.y = speedY;
        }
        return speed;
    }
}
