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
    public GameObject player;
    /// <summary>
    /// 生成したplayerオブジェクトのスクリプト
    /// </summary>
    private BreadPlayer playerScript;
    /// <summary>
    /// EnemyPrefab
    /// </summary>
    [SerializeField] private GameObject enemyPrefab;
    /// <summary>
    /// 生成したEnemyPrefab
    /// </summary>
    private GameObject enemy;
    /// <summary>
    /// enemyオブジェクトのスクリプト
    /// </summary>
    private BreadEnemy enemyScript;
    /// <summary>
    /// StonePrefab
    /// </summary>
    [SerializeField] private GameObject stonePrefab;
    /// <summary>
    /// 生成したStonePrefab
    /// </summary>
    private GameObject stone;
    /// <summary>
    /// マス目の大きさを決定する
    /// </summary>
    public Vector2 cellSize;

    void Start()
    {
        player = Instantiate(playerPrefab);
        playerScript = player.GetComponent<BreadPlayer>();
        playerScript.directer = GetComponent<BreadDirecter>();
        enemy = Instantiate(enemyPrefab);
        enemyScript = enemy.GetComponent<BreadEnemy>();
        enemyScript.directer = GetComponent<BreadDirecter>();
        stone = Instantiate(stonePrefab);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            playerScript.touch = Input.GetTouch(0).position;//スクリーン座標を記録
            playerScript.touch = Camera.main.ScreenToWorldPoint(playerScript.touch);//スクリーン座標をワールド座標に変換
            playerScript.touch.x = Mathf.RoundToInt(playerScript.touch.x);//マス目に合わせて整数化
            playerScript.touch.y = Mathf.RoundToInt(playerScript.touch.y);//マス目に合わせて整数化
        }
        if (Input.GetMouseButton(0))//開発用、ビルド時コメントアウト
        {
            playerScript.touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerScript.touch.x = Mathf.RoundToInt(playerScript.touch.x);//マス目に合わせて整数化
            playerScript.touch.y = Mathf.RoundToInt(playerScript.touch.y);//マス目に合わせて整数化
        }
    }
}
