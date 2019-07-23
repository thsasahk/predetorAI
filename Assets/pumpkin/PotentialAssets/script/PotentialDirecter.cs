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
    /// インスタンスしたslimeオブジェクト
    /// </summary>
    private GameObject slime;
    /// <summary>
    /// インスタンスしたfrogオブジェクト
    /// </summary>
    private GameObject frog;
    /// <summary>
    /// slimeオブジェクトのスクリプト
    /// </summary>
    private slimeAI slimeAI;
    /// <summary>
    /// frogオブジェクトのスクリプト
    /// </summary>
    private frogAI frogAI;

    void Start()
    {
        slime = Instantiate(slimePrefab);
        frog = Instantiate(frogPrefab);
        slimeAI = slime.GetComponent<slimeAI>();
        frogAI = frog.GetComponent<frogAI>();
    }

    void Update()
    {
        slimeAI.frogPosition = frog.transform.position;
        frogAI.slimePosition = slime.transform.position;
    }
}
