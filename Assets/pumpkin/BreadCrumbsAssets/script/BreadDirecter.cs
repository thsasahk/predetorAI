using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadDirecter : MonoBehaviour
{
    /// <summary>
    /// PlayerPrefab
    /// </summary>
    [SerializeField] private GameObject playerPrefab;
    /// <summary>
    /// 生成したPlayerPrefab
    /// </summary>
    private GameObject player;
    /// <summary>
    /// EnemyPrefab
    /// </summary>
    [SerializeField] private GameObject enemyPrefab;
    /// <summary>
    /// 生成したEnemyPrefab
    /// </summary>
    private GameObject enemy;
    /// <summary>
    /// StonePrefab
    /// </summary>
    [SerializeField] private GameObject stonePrefab;
    /// <summary>
    /// 生成したStonePrefab
    /// </summary>
    private GameObject stone;

    void Start()
    {
        player = Instantiate(playerPrefab);
        enemy = Instantiate(enemyPrefab);
        stone = Instantiate(stonePrefab);
    }

    void Update()
    {
        
    }
}
