using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batController : MonoBehaviour
{
    /// <summary>
    /// オブジェクトのRigidBody2D
    /// </summary>
    public Rigidbody2D rb2D;
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
    /// オブジェクトの速度
    /// </summary>
    public Vector2 speed;
    /// <summary>
    /// 進行方向の角度
    /// </summary>
    private float angle;
    /// <summary>
    /// 角度の変化値
    /// </summary>
    [SerializeField] private float thrusterPower;
    /// <summary>
    /// タッチ座標
    /// </summary>
    public Vector2 touch;
    /// <summary>
    /// 現在値を記録
    /// </summary>
    private Vector2 bPosition;
    /// <summary>
    /// オブジェクトから目標へのベクトル
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// 現在の進路ベクトルと目標へのベクトルの角度差
    /// </summary>
    private float targetAngle;
    /// <summary>
    /// 進路ベクトルと目標ベクトルの外積
    /// </summary>
    private Vector3 cross;
    /// <summary>
    /// 進路の修正値
    /// </summary>
    private float addAngle;
    /// <summary>
    /// ブレーキをかけるタイミング
    /// </summary>
    [SerializeField] private float brake;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        touch = gameObject.transform.position;//初期化
    }

    void Update()
    {
        speed = rb2D.velocity;//現在の速度を記録
        bPosition = gameObject.transform.position;//現在の位置を記録
        target = touch - bPosition;//目標地点への方向ベクトルを取得
        angle = transform.eulerAngles.z * (Mathf.PI / 180.0f);//自身の向いている方向角度をラジアン化、参考元→http://ftvoid.com/blog/post/631
        direction.x = Mathf.Cos(angle);//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(angle);//自身の方向ベクトルを取得
        if (Mathf.Abs(target.x) + Mathf.Abs(target.y) >= brake)//目標地点へ近づいたらブレーキをかける
        {
            SetThruster(direction.normalized, target.normalized);
            Drive(direction.normalized);
        }
        else
        {
            rb2D.AddForce(backPower * -speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 前進する
    /// </summary>
    /// <param name="t">進行方向ベクトル</param>
    private void Drive(Vector2 d)
    {
        rb2D.AddForce(power * d * Time.deltaTime);//tベクトル方向に力を加える
        rb2D.AddForce(backPower * (d - speed) * Time.deltaTime);//現在の進行方向と逆方向に力を加える、速度が大きいほど逆噴射も大きくなる。参考元→http://nnana-gamedev.hatenablog.com/entry/2017/09/07/012721
    }

    /// <summary>
    /// 自身の方向ベクトルを目標物への方向ベクトルに重ねるように調整する
    /// </summary>
    /// <param name="d">正規化したdirectionベクトル</param>
    /// <param name="t">targetベクトル</param>
    private void SetThruster(Vector2 d, Vector2 t)
    {
        targetAngle = Vector3.Angle(d, t);//自身の方向ベクトルと目標物への方向ベクトルの角度差を求める
        cross = Vector3.Cross(d, t);//自身の方向ベクトルと目標物への方向ベクトルの外積を求め、自身の方向ベクトルを基準とした場合の目標物への方向ベクトルの方角を明らかにする
        addAngle = cross.z * thrusterPower * Time.deltaTime;
        angle += addAngle;//両ベクトルが重なるように角度を加算減算する
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * angle);//オブジェクトにangle方向を向かせる
    }
}
