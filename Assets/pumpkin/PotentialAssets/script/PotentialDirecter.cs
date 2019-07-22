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

    void Start()
    {
        slime = Instantiate(slimePrefab);
        frog = Instantiate(frogPrefab);
    }

    void Update()
    {
        
    }
}
