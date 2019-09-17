using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCamera : MonoBehaviour
{
    /// <summary>
    /// GameDirecter
    /// </summary>
    [SerializeField] private GameObject directer;

    void Start()
    {
        Instantiate(directer);
    }

    void Update()
    {
        
    }
}
