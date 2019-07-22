using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialCamera : MonoBehaviour
{
    /// <summary>
    /// GameDirecterオブジェクト
    /// </summary>
    [SerializeField] private GameObject directer;

    void Start()
    {
        Instantiate(directer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
