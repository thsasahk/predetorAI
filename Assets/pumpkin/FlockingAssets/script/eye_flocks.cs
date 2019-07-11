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
    /// 群れの平均方向ベクトルに重なるように調整する力
    /// </summary>
    [SerializeField] private float fixPower;
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
    private float total;
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
    [SerializeField] private float sensorLength;
    /// <summary>
    /// センサーが作動したときの修正値
    /// </summary>
    [SerializeField] private float sensorPower;
    /// <summary>
    /// stoneオブジェクトへの距離
    /// </summary>
    private Vector2[] stoneLength;
    /// <summary>
    /// 進行予定の方向からstoneオブジェクトの方向への角度
    /// </summary>
    private float stoneAngle;
    /// <summary>
    /// stoneオブジェクトからdirectionベクトルへの最短距離
    /// </summary>
    private float length;
    /// <summary>
    /// コライダーの半径
    /// </summary>
    private float radius;
    /// <summary>
    /// 障害物の数
    /// </summary>
    public int sNumber;
    /// <summary>
    /// stoneオブジェクトのCircleCollider
    /// </summary>
    private CircleCollider2D circle;
    /// <summary>
    /// 障害物回避の際にコライダーの半径を大きく見積もる値
    /// </summary>
    [SerializeField] private float surplus;
    /// <summary>
    /// 舵修正角度の最大値
    /// </summary>
    [SerializeField] private float maxAngle;
    /// <summary>
    /// 障害物を無視してよい角度
    /// </summary>
    [SerializeField] private float saftyAngle;
    /// <summary>
    /// 回避する障害物の要素番号
    /// </summary>
    private int targetStone;
    /// <summary>
    /// 回避する障害物への距離
    /// </summary>
    private float targetLength;
    /// <summary>
    /// 最後に方向転換してからの時間
    /// </summary>
    private float time;
    /// <summary>
    /// 方向転換の時間の間隔
    /// </summary>
    [SerializeField] private float delay;

    private Vector2[] mPosition;

    private float memberAngle;

    [SerializeField] private float saftyLength;

    public bool safty;

    [SerializeField] private float suddenBraking;

    [SerializeField] private float saftyPower;

    private float totalBack;

    void Start()
    {
        startPosition.x = Random.Range(xMin, xMax);
        startPosition.y = Random.Range(yMin, yMax);
        stoneLength = new Vector2[sNumber];
        mPosition = new Vector2[2];
        transform.position = startPosition;
        rb2D = GetComponent<Rigidbody2D>();
        touch = startPosition;
        radius = stone[0].GetComponent<CircleCollider2D>().radius + surplus;//障害物の企画が一定ならこれでok
        direction.x = Mathf.Cos(angle);//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(angle);//自身の方向ベクトルを取得
    }

    void Update()
    {
        speed = rb2D.velocity;//現在の速度を記録
        angle = 0;//初期化
        ePosition = gameObject.transform.position;//現在の位置を記録
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
        for(int n = 0; n <= sNumber - 1; n++)
        {
            stoneLength[n].x = stone[n].transform.position.x - ePosition.x;
            stoneLength[n].y = stone[n].transform.position.y - ePosition.y;
        }
        if (Mathf.Abs(target.x) + Mathf.Abs(target.y) >= brake)//目標地点へ近づいたらブレーキをかける
        {
            if (time >= delay)
            {
                SetThruster(direction.normalized, target.normalized, vave.normalized);
                time = 0;
            }
            Drive(direction.normalized);
            time += Time.deltaTime;
        }
        else
        {
            rb2D.AddForce(totalBack * -speed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        number = 0;//初期化
        pave = Vector2.zero;//初期化
        vave = Vector2.zero;//初期化
        stoneAngle = 0;//初期化
        targetLength = sensorLength;//初期化
    }

    /// <summary>
    /// 前進する
    /// </summary>
    /// <param name="d">進行方向ベクトル</param>
    private void Drive(Vector2 d)
    {
        rb2D.AddForce(power * d * target.magnitude * Time.deltaTime);//dベクトル方向に力を加える
        rb2D.AddForce(totalBack * -speed * Time.deltaTime);//現在の進行方向と逆方向に力を加える、速度が大きいほど逆噴射も大きくなる。参考元→http://nnana-gamedev.hatenablog.com/entry/2017/09/07/012721
    }

    /// <summary>
    /// 自身の方向ベクトルを目標物への方向ベクトルに重ねるように調整する
    /// </summary>
    /// <param name="d">正規化したdirectionベクトル</param>
    /// <param name="t">targetベクトル</param>
    /// <param name="v">群れの平均速度ベクトル</param>
    private void SetThruster(Vector2 d, Vector2 t,Vector2 v)
    {
        //safty = true;//初期化
        totalBack = backPower;
        for (int n = 0; n <= sNumber - 1; n++)
        {
            if (stoneLength[n].magnitude < targetLength)//より近い障害物を回避する対象に設定する
            {
                stoneAngle = Vector3.Angle(d, stoneLength[n].normalized);
                targetLength= stoneLength[n].magnitude;
                targetStone = n;
            }
        }
        if (targetLength * Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * stoneAngle)) < radius &&//障害物の中心から進行方向ベクトルへ垂直に下したベクトルと障害物の半径を比較
            targetLength * Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * stoneAngle)) < sensorLength &&//障害物との接触予想点がセンサーの範囲内であるか確認
            Mathf.Abs(stoneAngle) <= saftyAngle)//背後の障害物には反応しない
        {
            angle -= Mathf.Sign(Vector3.Cross(d, stoneLength[targetStone].normalized).z) *
                sensorPower * stoneAngle /** Time.deltaTime*/;
            if (Mathf.Abs(angle) >= maxAngle)
            {
                angle = Mathf.Sign(angle) * maxAngle;
            }
            direction.x = Mathf.Cos(Mathf.Deg2Rad * (angle + transform.eulerAngles.z));//自身の方向ベクトルを取得
            direction.y = Mathf.Sin(Mathf.Deg2Rad * (angle + transform.eulerAngles.z));//自身の方向ベクトルを取得
        }
        if (angle != 0)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle + transform.eulerAngles.z);//オブジェクトにangle方向を向かせる
            return;
        }
        angle += Mathf.Sign(Vector3.Cross(d, t).z) * Vector3.Angle(d, t) * thrusterPower;
        angle += Mathf.Sign(Vector3.Cross(d, v).z) * Vector3.Angle(d, v) * fixPower;
        if (Mathf.Abs(angle) >= maxAngle)
        {
            angle = Mathf.Sign(angle) * maxAngle;
        }
        if (member[0] != null)
        {
            for (int m = 0; m <= number - 1; m++)
            {
                mPosition[1] = member[m].transform.position;
                if (mPosition[0].magnitude > mPosition[1].magnitude || m == 0)
                {
                    mPosition[0] = mPosition[1];
                }
            }
            memberAngle = Vector2.Angle(direction, mPosition[0] - ePosition);
            if (memberAngle <= saftyAngle)
            {
                angle -= Mathf.Sign(Vector3.Cross(direction, mPosition[0] - ePosition).z) * coefficient / memberAngle;
                totalBack += saftyPower;
            }
        }
        for (int i = 0; i <= number - 1; i++)
        {
            total += coefficient / (Vector3.Cross(d, distance[i]).z * distance[i].magnitude);//視界内のeyeオブジェクトへの距離が近い程影響が大きくなる
        }
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle + transform.eulerAngles.z);//オブジェクトにangle方向を向かせる
        direction.x = Mathf.Cos(Mathf.Deg2Rad * (angle + transform.eulerAngles.z));//自身の方向ベクトルを取得
        direction.y = Mathf.Sin(Mathf.Deg2Rad * (angle + transform.eulerAngles.z));//自身の方向ベクトルを取得
    }
}
