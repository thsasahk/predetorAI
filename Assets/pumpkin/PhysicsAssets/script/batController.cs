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
    /// 最高速度
    /// </summary>
    [SerializeField] private float maxSpeed;
    /// <summary>
    /// オブジェクトのローカルベクトル方向の速度
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// 速度の減衰率
    /// </summary>
    [SerializeField] private float damp;
    /// <summary>
    /// オブジェクトの角度
    /// </summary>
    private float angle;
    /// <summary>
    /// 角度の変化値
    /// </summary>
    [SerializeField] private float thrusterPower;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        angle = transform.eulerAngles.z * (Mathf.PI / 180.0f);//自身の角度をラジアン化、参考元→http://ftvoid.com/blog/post/631
    }

    void Update()
    {
        rb2D.velocity *= damp * Time.deltaTime;//フレームごとに速度を減衰させる
        speed = transform.TransformDirection(rb2D.velocity);//ローカルベクトル方向の速度を記録
        if (Input.GetButton("Horizontal"))
        {
            SetThruster(Mathf.Sign(Input.GetAxis("Horizontal")));
        }
        if (Input.GetButton("Vertical") && Mathf.Abs(speed.y) <= maxSpeed)
        {
            Drive(Mathf.Sign(Input.GetAxis("Vertical")));
        }
    }

    /// <summary>
    /// 進行方向を変更する
    /// </summary>
    /// <param name="h">水平入力の正負</param>
    private void SetThruster(float h)
    {
        angle += h * thrusterPower * Time.deltaTime;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));//自身の方向ベクトルを取得
        direction = direction.normalized;//方向ベクトルを正規化
    }

    /// <summary>
    /// ベクトル方向に直進する
    /// </summary>
    /// <param name="v">垂直方向の入力の正負</param>
    private void Drive(float v)
    {
        if (Mathf.Abs(speed.y) >= maxSpeed)//現在の速度の絶対値と最高速を比べる
        {
            speed.y = v * maxSpeed;//maxSpeed以上の速度にはしない
        }
        rb2D.AddForce(v * power * direction * Time.deltaTime);//ベクトル方向に力を加える
    }
}
