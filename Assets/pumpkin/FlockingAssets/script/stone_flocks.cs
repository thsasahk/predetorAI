using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone_flocks : MonoBehaviour
{
    /// <summary>
    /// x座標の最大値
    /// </summary>
    [SerializeField] private float maxHorizon;
    /// <summary>
    /// x座標の最小値
    /// </summary>
    [SerializeField] private float minHorizon;
    /// <summary>
    /// y座標の最大値
    /// </summary>
    [SerializeField] private float maxVertical;
    /// <summary>
    /// y座標の最小値
    /// </summary>
    [SerializeField] private float minVertical;

    void Start()
    {
        transform.position = new Vector2(Random.Range(minHorizon, maxHorizon), Random.Range(minVertical, maxVertical));
    }

    void Update()
    {
        
    }
}
