using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeAI : MonoBehaviour
{
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

    void Start()
    {
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
