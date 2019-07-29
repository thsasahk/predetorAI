using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialDirecter : MonoBehaviour
{
    /// <summary>
    /// slimeプレファブオブジェクト
    /// </summary>
    [SerializeField] private GameObject slimePrefab;
    /// <summary>
    /// frogプレファブオブジェクト
    /// </summary>
    [SerializeField] private GameObject frogPrefab;
    /// <summary>
    /// capsuleプレファブオブジェクト
    /// </summary>
    [SerializeField] private GameObject capsulePrefab;
    /// <summary>
    /// インスタンスしたslimeオブジェクト
    /// </summary>
    public GameObject[] slime;
    /// <summary>
    /// slimeオブジェクトの数
    /// </summary>
    public int sNumber;
    /// <summary>
    /// インスタンスしたfrogオブジェクト
    /// </summary>
    private GameObject frog;
    /// <summary>
    /// インスタンスしたcapsuleオブジェクトの配列
    /// </summary>
    public GameObject[] capsule;
    /// <summary>
    /// capsule配列の要素数
    /// </summary>
    public int cNumber;
    /// <summary>
    /// slimeオブジェクトのスクリプト
    /// </summary>
    private slimeAI[] slimeAI;
    /// <summary>
    /// frogオブジェクトのスクリプト
    /// </summary>
    private frogAI frogAI;
    /// <summary>
    /// capsuleオブジェクトの配置の最小値
    /// </summary>
    [SerializeField] private Vector2 minPosition;
    /// <summary>
    /// capsuleオブジェクトの配置の最大値
    /// </summary>
    [SerializeField] private Vector2 maxPosition;
    /// <summary>
    /// リーダーを用意するかどうかを決定する
    /// </summary>
    [SerializeField] private bool leader;
    /// <summary>
    /// slimeオブジェクトからfrogオブジェクトへの距離
    /// </summary>
    private float distance;
    /// <summary>
    /// もっとも近いslimeオブジェクトからfrogオブジェクトへの距離
    /// </summary>
    private float minDis;
    /// <summary>
    /// リーダーオブジェクトの要素番号
    /// </summary>
    private int leaderNumber;

    void Start()
    {
        slime = new GameObject[sNumber];
        slimeAI = new slimeAI[sNumber];
        for (int n = 0; n < sNumber; n++)
        {
            slime[n] = Instantiate(slimePrefab);//slimePrefabを生成、slime変数に格納
            slimeAI[n] = slime[n].GetComponent<slimeAI>();
            slimeAI[n].directer = gameObject;
        }
        frog = Instantiate(frogPrefab);//frogPrefabを生成、frog変数に格納
        capsule = new GameObject[cNumber];
        for (int m = 0; m < cNumber; m++)
        {
            capsule[m] = Instantiate(capsulePrefab,
                new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)),
                Quaternion.identity);
        }
        frogAI = frog.GetComponent<frogAI>();
        frogAI.directer = gameObject;
    }

    void Update()
    {
        if (leader)
        {
            distance = 0;//初期化
            for (int n = 0; n < sNumber; n++)//slime配列内のオブジェクトとflogオブジェクトの距離を一つずつ比べる
            {
                slimeAI[n].isLeader = false;//初期化
                distance = (slime[n].transform.position - frog.transform.position).magnitude;
                if (distance < minDis || distance == 0)
                {
                    minDis = distance;
                    leaderNumber = n;
                }
            }
            slimeAI[leaderNumber].isLeader = true;
        }
        for (int n = 0; n < sNumber; n++)
        {
            slimeAI[n].frogPosition = frog.transform.position;//flogオブジェクトのpositionを受け渡す
            frogAI.slimePosition = slime[n].transform.position;//slimeオブジェクトのpositionを受け渡す
        }
    }
}
