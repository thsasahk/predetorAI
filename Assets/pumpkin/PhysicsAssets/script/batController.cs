using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batController : MonoBehaviour
{
    /// <summary>
    /// オブジェクトのRigidBody2D
    /// </summary>
    private Rigidbody2D rb2D;
    /// <summary>
    /// 推進力
    /// </summary>
    [SerializeField] private float power;
    /// <summary>
    /// オブジェクトが向いてる方向ベクトル
    /// </summary>
    private Vector2 direction;
    /// <summary>
    /// 速度制限の為の逆噴射
    /// </summary>
    [SerializeField] private float backPower;
    /// <summary>
    /// オブジェクトのローカルベクトル方向の速度
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// 進行方向の角度
    /// </summary>
    private float angle;
    /// <summary>
    /// 角度の変化値
    /// </summary>
    [SerializeField] private float thrusterPower;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speed = rb2D.velocity;//現在の速度を記録
        SetThruster(Input.GetAxis("Horizontal"));
        Drive(Input.GetAxis("Vertical"), direction.normalized);
    }

    /// <summary>
    /// 進行方向を変更する
    /// </summary>
    /// <param name="h">水平入力</param>
    private void SetThruster(float h)
    {
        angle = transform.eulerAngles.z * (Mathf.PI / 180.0f);//自身の向いている方向角度をラジアン化、参考元→http://ftvoid.com/blog/post/631
        angle += h * thrusterPower * Time.deltaTime;//水平入力をもとに進行方向を変更
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * angle);//オブジェクトに進行方向を向かせる
        direction.x = Mathf.Cos(angle);//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(angle);//自身の方向ベクトルを取得
    }

    /// <summary>
    /// ベクトル方向に直進する
    /// </summary>
    /// <param name="v">垂直方向の入力</param>
    private void Drive(float v,Vector2 d)
    {
        rb2D.AddForce(v * power * d * Time.deltaTime);//directionベクトル方向に力を加える
        rb2D.AddForce(backPower * (v * d - speed) * Time.deltaTime);//現在の進行方向と逆方向に力を加える、速度が大きいほど逆噴射も大きくなる。参考元→http://nnana-gamedev.hatenablog.com/entry/2017/09/07/012721
    }
}
