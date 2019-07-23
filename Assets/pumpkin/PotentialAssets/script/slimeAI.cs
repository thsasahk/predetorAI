using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeAI : MonoBehaviour
{
    /// <summary>
    /// 自身のRigidbody2D
    /// </summary>
    private Rigidbody2D rb2D;
    /// <summary>
    /// 自身の位置
    /// </summary>
    private Vector2 slimePosition;
    /// <summary>
    /// flogオブジェクトのposition
    /// </summary>
    public Vector2 frogPosition;
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

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        startPosition.x = Random.Range(minPosition.x, maxPosition.x);//位置をランダムに決定
        startPosition.y = Random.Range(minPosition.y, maxPosition.y);//位置をランダムに決定
        transform.position = startPosition;
    }

    void Update()
    {
        slimePosition = transform.position;
        target = (slimePosition - frogPosition).normalized;
        distance = (slimePosition - frogPosition).magnitude;
    }
}
