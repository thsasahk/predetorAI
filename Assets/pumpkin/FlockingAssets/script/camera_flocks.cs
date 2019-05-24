using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_flocks : MonoBehaviour
{
    /// <summary>
    /// GameDirecterオブジェクト
    /// </summary>
    [SerializeField] private GameObject gameDirecter;

    void Start()
    {
        Instantiate(gameDirecter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
