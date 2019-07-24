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

    private Vector2 touchPosition;
    /// ターゲットへの方向ベクトル
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// ターゲットへの距離
    /// </summary>
    private float distance;
    /// <summary>
    /// ポテンシャル関数に使用する変数、力の合計
    /// </summary>
    private float U;
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

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<CircleCollider2D>();
        startPosition.x = Random.Range(minPosition.x, maxPosition.x);//位置をランダムに決定
        startPosition.y = Random.Range(minPosition.y, maxPosition.y);//位置をランダムに決定
        transform.position = startPosition;
    }

    void Update()
    {
        frogPosition = transform.position;
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
        rb2D.AddForce(Potencial() * target * Time.deltaTime);
    }

    /// <summary>
    /// ポテンシャル関数を使ってオブジェクトに与える力の大きさを計算する
    /// </summary>
    /// <returns></returns>
    private float Potencial()
    {
        d = distance / cc2D.radius;
        if (d <= dMin)//Uの値が急激に大きくなってしまうのを防ぐ
        {
            d = dMin;
        }
        else if (d >= dMax)
        {
            d = dMax;
        }
        U = -A / Mathf.Pow(d, n) + B / Mathf.Pow(d, m);//-A / Mathf.Pow(d, n)で対象からの引力、B / Mathf.Pow(d, m)で対象からの斥力を計算する
        return U;
    }
}
