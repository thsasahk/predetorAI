using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogAI : MonoBehaviour
{
    /// <summary>
    /// 自身のRigidbody2D
    /// </summary>
    private Rigidbody2D rb2D;
    /// <summary>
    /// 自身のCircleCollider2D
    /// </summary>
    private CircleCollider2D cc2D;
    /// <summary>
    /// slimeオブジェクトのposition
    /// </summary>
    public Vector2 slimePosition;
    /// <summary>
    /// start時の位置
    /// </summary>
    private Vector2 startPosition;
    /// <summary>
    /// start時の位置の最小値
    /// </summary>
    [SerializeField] private Vector2 minPosition;
    /// <summary>
    /// start時の位置の最大値
    /// </summary>
    [SerializeField] private Vector2 maxPosition;
    /// <summary>
    /// 自身の位置
    /// </summary>
    private Vector2 frogPosition;
    /// <summary>
    /// タッチした位置
    /// </summary>
    private Vector2 touchPosition;
    /// <summary>
    /// ターゲットへの方向ベクトル
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// ターゲットへの距離
    /// </summary>
    private float distance;
    /// <summary>
    /// ポテンシャル関数に使用する変数、引力の大きさに影響
    /// </summary>
    [SerializeField] private float A;
    /// <summary>
    /// ポテンシャル関数に使用する変数、斥力の大きさに影響
    /// </summary>
    [SerializeField] private float B;
    /// <summary>
    /// ポテンシャル関数に使用する変数、引力の大きさに影響
    /// </summary>
    [SerializeField] private float n;
    /// <summary>
    /// ポテンシャル関数に使用する変数、斥力の大きさに影響
    /// </summary>
    [SerializeField] private float m;
    /// <summary>
    /// ポテンシャル関数に使用する変数、ユニット間の距離をユニットの長さの単位で取得(ユニット何個分みたいな)
    /// </summary>
    private float d;
    /// <summary>
    /// d変数の最小値
    /// </summary>
    [SerializeField] private float dMin;
    /// <summary>
    /// 変数dの最大値
    /// </summary>
    [SerializeField] private float dMax;
    /// <summary>
    /// 自身の速度
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// 制限速度を変化させる関数
    /// </summary>
    [SerializeField] private float coefficient;
    /// <summary>
    /// potentialDirecterオブジェクト
    /// </summary>
    public GameObject directer;
    /// <summary>
    /// potentialDirecterオブジェクトのスクリプト
    /// </summary>
    private PotentialDirecter directerScript;
    /// <summary>
    /// Avoid関数に使用する変数、斥力の大きさに影響
    /// </summary>
    [SerializeField] private float avoidB;
    /// <summary>
    /// Avoid関数に使用する変数、斥力の大きさに影響
    /// </summary>
    [SerializeField] private float avoidM;
    /// <summary>
    /// 斥力を計算する障害物の位置
    /// </summary>
    private Vector2 capsulPosition;
    /// <summary>
    /// 障害物から自身への方向ベクトル
    /// </summary>
    private Vector2 capVec;
    /// <summary>
    /// 障害物への距離
    /// </summary>
    private float capDis;
    /// <summary>
    /// 障害物を回避する際に力を加える方向ベクトル
    /// </summary>
    private Vector2 thruster;
    /// <summary>
    /// 障害物を回避する角度条件
    /// </summary>
    [SerializeField] private float angle;
    /// <summary>
    /// 角度条件を満たす中で、最も近い障害物
    /// </summary>
    private float minDis;
    /// <summary>
    /// 角度条件を満たす中で、最も近い障害物から自身への方向ベクトル
    /// </summary>
    private Vector2 minVec;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<CircleCollider2D>();
        startPosition.x = Random.Range(minPosition.x, maxPosition.x);//位置をランダムに決定
        startPosition.y = Random.Range(minPosition.y, maxPosition.y);//位置をランダムに決定
        transform.position = startPosition;
        touchPosition = startPosition;//初期値
        directerScript = directer.GetComponent<PotentialDirecter>();
    }

    void Update()
    {
        frogPosition = transform.position;
        speed = rb2D.velocity;
        minVec = Vector2.zero;//初期化
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
        }
        if (Input.GetMouseButton(0))//開発用、ビルド時コメントアウト
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        target = (frogPosition - touchPosition).normalized;
        distance = (frogPosition - touchPosition).magnitude;
        for (int m = 0; m < directerScript.cNumber; m++)//回避する方向と力の大きさを障害物ごとに決める
        {
            capsulPosition = directerScript.capsule[m].transform.position;
            capVec = (frogPosition - capsulPosition).normalized;
            capDis = (frogPosition - capsulPosition).magnitude;
            if (Vector3.Angle(target, capVec) <= angle && (minVec == Vector2.zero || 
                capDis < minDis))//
            {
                minVec = capVec;
                minDis = capDis;
            }
        }
        if (Vector3.Cross(target, minVec).z < 0)//内積の正負で障害物を左右どちら周りで回避するか決定する
        {
            thruster.x = target.y;
            thruster.y = -target.x;
        }
        else
        {
            thruster.x = -target.y;
            thruster.y = target.x;
        }
        rb2D.AddForce((Potencial() * (coefficient * target + speed) + Avoid() * thruster) * Time.deltaTime);
    }

    /// <summary>
    /// ポテンシャル関数を使ってオブジェクトに与える力の大きさを計算する
    /// </summary>
    /// <returns></returns>
    private float Potencial()
    {
        d = distance / (2 * cc2D.radius);
        if (d <= dMin)//Uの値が急激に大きくなってしまうのを防ぐ
        {
            d = dMin;
        }
        else if (d >= dMax)
        {
            d = dMax;
        }
        return -A / Mathf.Pow(d, n) + 
            B / Mathf.Pow(d, m);//-A / Mathf.Pow(d, n)で対象からの引力、B / Mathf.Pow(d, m)で対象からの斥力を計算する
    }

    private float Avoid()
    {
        d = minDis / (2 * cc2D.radius);
        if (d <= dMin)//Uの値が急激に大きくなってしまうのを防ぐ
        {
            d = dMin;
        }
        return avoidB / Mathf.Pow(d, avoidM);
    }
}
