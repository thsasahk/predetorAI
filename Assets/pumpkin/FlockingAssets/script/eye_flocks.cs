using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eye_flocks : MonoBehaviour
{
    /// <summary>
    /// 自身のpositionを記録
    /// </summary>
    private Vector2 position;
    /// <summary>
    /// 自身の方向ベクトル
    /// </summary>
    public Vector2 direction;
    /// <summary>
    /// 視界内のオブジェクトの方向ベクトル
    /// </summary>
    public Vector2[] directions;
    /// <summary>
    /// 視界の長さ
    /// </summary>
    public float viewLength;
    /// <summary>
    /// 視界の角度
    /// </summary>
    public float viewAngle;
    /// <summary>
    /// 視界内の仲間
    /// </summary>
    public GameObject[] member;
    /// <summary>
    /// オブジェクトの角度
    /// </summary>
    private float angle;
    /// <summary>
    /// member配列に格納されているオブジェクトの数
    /// </summary>
    public int number = 0;
    /// <summary>
    /// スタート時のポジション
    /// </summary>
    private Vector2 startPosition;
    /// <summary>
    /// スタート時のポジションのx最小値
    /// </summary>
    [SerializeField] private float xMin;
    /// <summary>
    /// スタート時のポジションのx最大値
    /// </summary>
    [SerializeField] private float xMax;
    /// <summary>
    /// スタート時のポジションのy最小値
    /// </summary>
    [SerializeField] private float yMin;
    /// <summary>
    /// スタート時のポジションのy最大値
    /// </summary>
    [SerializeField] private float yMax;
    /// <summary>
    /// タッチ情報
    /// </summary>
    public Vector2 touch;
    /// <summary>
    /// オブジェクトの速度
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// オブジェクトのRigidBody2D
    /// </summary>
    public Rigidbody2D rb2D;
    /// <summary>
    /// 現在値を記録
    /// </summary>
    private Vector2 ePosition;
    /// <summary>
    /// オブジェクトから目標へのベクトル
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// ブレーキをかけるタイミング
    /// </summary>
    [SerializeField] private float brake;
    /// <summary>
    /// 推進力
    /// </summary>
    [SerializeField] private float power;
    /// <summary>
    /// 速度制限の為の逆噴射
    /// </summary>
    [SerializeField] private float backPower;
    /// <summary>
    /// 角度の変化値
    /// </summary>
    [SerializeField] private float thrusterPower;
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
    /// 視界内のユニットの平均位置ベクトル
    /// </summary>
    private Vector2 pave;
    /// <summary>
    /// 視界内のユニットの平均速度ベクトル
    /// </summary>
    private Vector2 vave;
    /// <summary>
    /// 視界内のeyeオブジェクトへのベクトル
    /// </summary>
    public Vector2[] distance;
    /// <summary>
    /// 係数
    /// </summary>
    [SerializeField] private float coefficient;
    /// <summary>
    /// 衝突回避を開始する条件
    /// </summary>
    [SerializeField] private float collisionLimit;
    /// <summary>
    /// eyeオブジェクトとの衝突回避の条件
    /// </summary>
    float total = 0;
    /// <summary>
    /// 障害物
    /// </summary>
    public GameObject[] stone;
    /// <summary>
    /// 障害物を検知するセンサー
    /// </summary>
    private Vector2 sensor;
    /// <summary>
    /// センサーの長さ
    /// </summary>
    [SerializeField] private Vector2 sensorLength;

    void Start()
    {
        startPosition.x = Random.Range(xMin, xMax);
        startPosition.y = Random.Range(yMin, yMax);
        transform.position = startPosition;
        rb2D = GetComponent<Rigidbody2D>();
        touch = startPosition;
    }

    void Update()
    {
        total = 0;//初期化
        speed = rb2D.velocity;//現在の速度を記録
        ePosition = gameObject.transform.position;//現在の位置を記録
        sensor = sensorLength * speed.normalized + ePosition;//センサーの長さ*センサーの方向+センサーのゼロ点
        for(int i = 0; i <= number - 1; i++)
        {
            pave.x += member[i].transform.position.x;//視界内のオブジェクトのx座標を合計する
            pave.y += member[i].transform.position.y;//視界内のオブジェクトのy座標を合計する
            vave.x += directions[i].x;//視界内のオブジェクトの方向ベクトルのx値を合計する
            vave.y += directions[i].y;//視界内のオブジェクトの方向ベクトルのy値を合計する
        }
        pave += ePosition;//自らの位置ベクトルを加算する
        vave += direction;//自らの方向ベクトルを加算する
        pave /= (number + 1);//要素数に自身を加えた値で除算して平均値を算出する
        vave /= (number + 1);//要素数に自身を加えた値で除算して平均値を算出する
        target = touch - ePosition;//目標地点への方向ベクトルを取得
        angle = transform.eulerAngles.z * (Mathf.PI / 180.0f);//自身の向いている方向角度をラジアン化、参考元→http://ftvoid.com/blog/post/631
        direction.x = Mathf.Cos(angle);//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(angle);//自身の方向ベクトルを取得
        if (Mathf.Abs(target.x) + Mathf.Abs(target.y) >= brake)//目標地点へ近づいたらブレーキをかける
        {
            SetThruster(direction.normalized, target.normalized, vave.normalized);
            Drive(direction.normalized);
        }
        else
        {
            rb2D.AddForce(backPower * -speed * Time.deltaTime);
        }
        //Debug.Log(touch);
    }

    private void LateUpdate()
    {
        //Debug.Log(number);
        //Debug.Log(pave);
        //Debug.Log(vave);
        number = 0;//初期化
        pave = Vector2.zero;//初期化
        vave = Vector2.zero;//初期化
    }

    /// <summary>
    /// 前進する
    /// </summary>
    /// <param name="t">進行方向ベクトル</param>
    private void Drive(Vector2 d)
    {
        rb2D.AddForce(power * d * (touch - pave).magnitude * Time.deltaTime);//tベクトル方向に力を加える
        rb2D.AddForce(backPower * (d - speed) * Time.deltaTime);//現在の進行方向と逆方向に力を加える、速度が大きいほど逆噴射も大きくなる。参考元→http://nnana-gamedev.hatenablog.com/entry/2017/09/07/012721
    }

    /// <summary>
    /// 自身の方向ベクトルを目標物への方向ベクトルに重ねるように調整する
    /// </summary>
    /// <param name="d">正規化したdirectionベクトル</param>
    /// <param name="t">targetベクトル</param>
    private void SetThruster(Vector2 d, Vector2 t,Vector2 v)
    {
        /*
        if(Vector3.Angle(d, t)> Vector3.Angle(d, v))
        {
            targetAngle = Vector3.Angle(d, t);//自身の方向ベクトルと目標物への方向ベクトルの角度差を求める
            cross = Vector3.Cross(d, t);//自身の方向ベクトルと目標物への方向ベクトルの外積を求め、自身の方向ベクトルを基準とした場合の目標物への方向ベクトルの方角を明らかにする
        }
        else
        {
            targetAngle = Vector3.Angle(d, v);//自身の方向ベクトルと目標物への方向ベクトルの角度差を求める
            cross = Vector3.Cross(d, v);//自身の方向ベクトルと目標物への方向ベクトルの外積を求め、自身の方向ベクトルを基準とした場合の目標物への方向ベクトルの方角を明らかにする
        }
        */
        //targetAngle = Vector3.Angle(d, t);//自身の方向ベクトルと目標物への方向ベクトルの角度差を求める
        //cross = Vector3.Cross(d, t);//自身の方向ベクトルと目標物への方向ベクトルの外積を求め、自身の方向ベクトルを基準とした場合の目標物への方向ベクトルの方角を明らかにする
        for (int i = 0; i <= number - 1; i++)
        {
            total += coefficient / (Vector3.Cross(d, distance[i]).z * distance[i].magnitude);//視界内のeyeオブジェクトへの距離が近い程影響が大きくなる
        }
        if (total >= collisionLimit)
        {
            targetAngle = Vector3.Cross(d, t).z * Vector3.Angle(d, t) + Vector3.Cross(d, v).z * Vector3.Angle(d, v)
            - total;
        }
        else
        {
            targetAngle = Vector3.Cross(d, t).z * Vector3.Angle(d, t) + Vector3.Cross(d, v).z * Vector3.Angle(d, v);
        }
        //targetAngle = Vector3.Cross(d, t).z * Vector3.Angle(d, t) + Vector3.Cross(d, v).z * Vector3.Angle(d, v)
        //    - m;
        addAngle = targetAngle * thrusterPower * Time.deltaTime;
        angle += addAngle;//両ベクトルが重なるように角度を加算減算する
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * angle);//オブジェクトにangle方向を向かせる
    }
}
