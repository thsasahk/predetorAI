using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireController : MonoBehaviour
{
    /// <summary>
    /// オブジェクトのRigidbody2D
    /// </summary>
    private Rigidbody2D rb2D;
    /// <summary>
    /// オブジェクトのposition
    /// </summary>
    private Vector2 fPosition;
    /// <summary>
    /// 推進力
    /// </summary>
    [SerializeField] private float power;
    /// <summary>
    /// 速度制限の為の逆噴射
    /// </summary>
    [SerializeField] private float backPower;
    /// <summary>
    /// オブジェクトの方向ベクトル
    /// </summary>
    private Vector2 direction;
    /// <summary>
    /// batオブジェクトのposition
    /// </summary>
    public Vector2 bPosition;
    /// <summary>
    /// fireオブジェクトからbatオブジェクトへの方向ベクトル
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// オブジェクトの速度
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// オブジェクトの角度
    /// </summary>
    private float angle;
    /// <summary>
    /// directionとtargetの角度差
    /// </summary>
    private float targetAngle;
    /// <summary>
    /// directionとtargetの外積
    /// </summary>
    private Vector3 cross;
    /// <summary>
    /// 角度の変化値
    /// </summary>
    private float addAngle;
    /// <summary>
    /// 角度の変化値の調整に使用する
    /// </summary>
    [SerializeField] private float thrusterPower;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speed = rb2D.velocity;//現在の速度を記録
        fPosition = gameObject.transform.position;//現在の位置を記録
        target = (bPosition - fPosition).normalized;//目標物への方向ベクトルを取得し正規化
        angle = transform.eulerAngles.z * (Mathf.PI / 180.0f);//自身の向いている方向角度をラジアン化、参考元→http://ftvoid.com/blog/post/631
        direction.x = Mathf.Cos(angle);//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(angle);//自身の方向ベクトルを取得
        SetThruster(direction.normalized,target);
        Drive();
    }

    /// <summary>
    /// 自身の方向ベクトルを目標物への方向ベクトルに重ねるように調整する
    /// </summary>
    /// <param name="d">正規化したdirectionベクトル</param>
    /// <param name="t">targetベクトル</param>
    private void SetThruster(Vector2 d,Vector2 t)
    {
        targetAngle = Vector3.Angle(d, t);//自身の方向ベクトルと目標物への方向ベクトルの角度差を求める
        cross = Vector3.Cross(d, t);//自身の方向ベクトルと目標物への方向ベクトルの外積を求め、自身の方向ベクトルを基準とした場合の目標物への方向ベクトルの方角を明らかにする
        addAngle = cross.z * thrusterPower * Time.deltaTime;
        angle += addAngle;//両ベクトルが重なるように角度を加算減算する
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * angle);//オブジェクトにangle方向を向かせる
    }

    private void Drive()
    {
        rb2D.AddForce(power * target * Time.deltaTime);//targetベクトル方向に力を加える
        rb2D.AddForce(backPower * (target - speed) * Time.deltaTime);//現在の進行方向と逆方向に力を加える、速度が大きいほど逆噴射も大きくなる。参考元→http://nnana-gamedev.hatenablog.com/entry/2017/09/07/012721
    }
}
