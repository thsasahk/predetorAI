using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directer : MonoBehaviour
{
    /// <summary>
    /// batPrefabオブジェクト
    /// </summary>
    [SerializeField] private GameObject batPrefab;
    /// <summary>
    /// fireオブジェクトのスクリプト
    /// </summary>
    private fireController fireController;
    /// <summary>
    /// firePrefabオブジェクト
    /// </summary>
    [SerializeField] private GameObject firePrefab;
    /// <summary>
    /// インスタンスしたbatオブジェクト
    /// </summary>
    private GameObject bat;
    /// <summary>
    /// インスタンスしたfireオブジェクト
    /// </summary>
    private GameObject fire;
    /// <summary>
    /// batオブジェクトのポジション
    /// </summary>
    [SerializeField] private Vector2 bPosition;
    /// <summary>
    /// fireオブジェクトのポジション
    /// </summary>
    [SerializeField] private Vector2 fPosition;

    void Start()
    {
        bat = Instantiate(batPrefab, bPosition, Quaternion.identity);
        fire = Instantiate(firePrefab, fPosition, Quaternion.identity);
        fireController = fire.GetComponent<fireController>();
    }

    // Update is called once per frame
    void Update()
    {
        fireController.bPosition = bat.transform.position;
    }
}
