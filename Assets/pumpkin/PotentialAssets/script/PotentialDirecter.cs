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
    private GameObject slime;
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
    private slimeAI slimeAI;
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

    void Start()
    {
        slime = Instantiate(slimePrefab);//slimePrefabを生成、slime変数に格納
        frog = Instantiate(frogPrefab);//frogPrefabを生成、frog変数に格納
        capsule = new GameObject[cNumber];
        for (int m = 0; m < cNumber; m++)
        {
            capsule[m] = Instantiate(capsulePrefab,
                new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)),
                Quaternion.identity);
        }
        slimeAI = slime.GetComponent<slimeAI>();
        slimeAI.directer = gameObject;
        frogAI = frog.GetComponent<frogAI>();
        frogAI.directer = gameObject;
    }

    void Update()
    {
        slimeAI.frogPosition = frog.transform.position;//flogオブジェクトのpositionを受け渡す
        frogAI.slimePosition = slime.transform.position;//slimeオブジェクトのpositionを受け渡す
    }
}
