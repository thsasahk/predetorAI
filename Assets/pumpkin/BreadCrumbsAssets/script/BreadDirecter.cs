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
    /// playerオブジェクトのposition
    /// </summary>
    private Vector2 playerPos;
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
    /// enemyオブジェクトのposition
    /// </summary>
    private Vector2 enemyPos;
    /// <summary>
    /// StonePrefab
    /// </summary>
    [SerializeField] private GameObject stonePrefab;
    /// <summary>
    /// 生成したStonePrefab
    /// </summary>
    private GameObject[] stone;
    /// <summary>
    /// stone配列の要素数
    /// </summary>
    public int stoneNumber;
    /// <summary>
    /// マス目の大きさを決定する
    /// </summary>
    public Vector2 cellSize;
    /// <summary>
    /// stoneオブジェクトのポジション
    /// </summary>
    private Vector2 stonePos;
    /// <summary>
    /// stoneオブジェクトのposition.xの最小値
    /// </summary>
    [SerializeField] private int minPosX;
    /// <summary>
    /// stoneオブジェクトのposition.yの最小値
    /// </summary>
    [SerializeField] private int minPosY;
    /// <summary>
    /// stoneオブジェクトのposition.xの最大値
    /// </summary>
    [SerializeField] private int maxPosX;
    /// <summary>
    /// stoneオブジェクトのposition.yの最大値
    /// </summary>
    [SerializeField] private int maxPosY;
    /// <summary>
    /// stoneオブジェクトのpositionを格納する配列
    /// </summary>
    private Vector2[] _stonePos;
    /// <summary>
    /// playerオブジェクトの移動ログ
    /// </summary>
    public Vector2[] trail;
    /// <summary>
    /// trail配列の要素数
    /// </summary>
    public int maxTrail;

    void Start()
    {
        player = Instantiate(playerPrefab);
        playerScript = player.GetComponent<BreadPlayer>();
        playerScript.directer = GetComponent<BreadDirecter>();
        playerPos = playerScript.startCell;
        enemy = Instantiate(enemyPrefab);
        enemyScript = enemy.GetComponent<BreadEnemy>();
        enemyScript.directer = GetComponent<BreadDirecter>();
        enemyPos = enemyScript.startCell;
        stone = new GameObject[stoneNumber];
        _stonePos = new Vector2[stoneNumber];
        trail = new Vector2[maxTrail];
        for(int n = 0; n < stoneNumber; n++)
        {
            stonePos.x = Random.Range(minPosX, maxPosX + 1);//stoneオブジェクトの配置をランダムに決定
            stonePos.y = Random.Range(minPosY, maxPosY + 1);//stoneオブジェクトの配置をランダムに決定
            _stonePos[n] = stonePos;
            for (int i = 0; i <= n; i++)
            {
                if ((stonePos == _stonePos[i] || stonePos == playerPos || stonePos == enemyPos) && i != n)
                    //生成済みの各オブジェクトと被らないように
                {
                    n--;//被ったら配列番号を戻してやり直し
                    break;
                }
                else
                {
                    if (i == n)
                    {
                        stone[n] = Instantiate(stonePrefab, stonePos, Quaternion.identity);
                        //自分の1つ前の配列番号までpositionが被らなかったら生成
                    }
                }
            }
        }
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
