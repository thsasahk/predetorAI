using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directer_flocks : MonoBehaviour
{

    [SerializeField] private GameObject eye;
    /// <summary>
    /// eyeオブジェクトの配列
    /// </summary>
    private GameObject[] eyes;
    /// <summary>
    /// eye配列の要素数
    /// </summary>
    [SerializeField] private int elements;

    void Start()
    {
        eyes = new GameObject[elements];
        for(int i = 0; i <= elements - 1; i++)
        {
            eyes[i] = Instantiate(eye);
        }
    }

    void Update()
    {
        
    }
}
