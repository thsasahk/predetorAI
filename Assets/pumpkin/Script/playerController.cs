using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    /// <summary>
    /// オブジェクトの移動方向と速度
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

    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.Translate(Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="h">左右の入力情報</param>
    /// <param name="v">上下の入力情報</param>
    /// <returns></returns>
    private Vector2 Move(float h,float v)
    {
        speed.x = h * speedX;
        speed.y = v * speedY;
        return speed * Time.deltaTime;
    }
}
