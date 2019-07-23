using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogAI : MonoBehaviour
{
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
        startPosition.x = Random.Range(minPosition.x, maxPosition.x);//位置をランダムに決定
        startPosition.y = Random.Range(minPosition.y, maxPosition.y);//位置をランダムに決定
    }

    void Update()
    {
        
    }
}
