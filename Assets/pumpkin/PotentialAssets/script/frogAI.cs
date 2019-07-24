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
        
    }
}
