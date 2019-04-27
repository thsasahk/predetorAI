using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    /// <summary>
    /// playerオブジェクトプレファブ
    /// </summary>
    [SerializeField]private GameObject playerPrefab;
    /// <summary>
    /// dragonオブジェクトプレファブ
    /// </summary>
    [SerializeField]private GameObject dragonPrefab;
    /// <summary>
    /// インスタンスしたplayerオブジェクト
    /// </summary>
    private GameObject player;
    /// <summary>
    /// インスタンスしたdragonオブジェクト
    /// </summary>
    private GameObject dragon;
    /// <summary>
    /// playerオブジェクトのポジション
    /// </summary>
    public Vector2 pPosition;
    /// <summary>
    /// dragonオブジェクトのポジション
    /// </summary>
    public Vector2 dPosition;
    /// <summary>
    /// dragonオブジェクトのスクリプト
    /// </summary>
    private dragonAI dragonAI;
    /// <summary>
    /// tileオブジェクト
    /// </summary>
    [SerializeField] private GameObject tile;

    void Start()
    {
        player = Instantiate(playerPrefab);//playerPrefabをインスタンスしてplayerに格納
        player.transform.position = pPosition;//インスタンスしたplayerオブジェクトのpositionを変更
        dragon = Instantiate(dragonPrefab);//dragonPrefabをインスタンスしてdragonに格納
        dragon.transform.position = dPosition;//インスタンスしたdragonオブジェクトのpositionを変更
        dragonAI = dragon.GetComponent<dragonAI>();//インスタンスしたdragonオブジェクトのスクリプトを取得
        Instantiate(tile, Vector2.zero, Quaternion.identity);//tileオブジェクトを生成
    }

    // Update is called once per frame
    void Update()
    {
        dPosition = dragon.transform.position;//現在のdragonオブジェクトの位置を記録
        pPosition = player.transform.position;//現在のplayerオブジェクトの位置を記録
        dragonAI.pPosition = pPosition;//dragonAIに現在のplayerの位置を受け渡し
    }
}
