using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour
{
    /// <summary>
    /// GameDirecterオブジェクト
    /// </summary>
    [SerializeField] private GameObject gameDirecter;

    void Start()
    {
        Instantiate(gameDirecter);
    }

    void Update()
    {
        
    }
}
