using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDirecter : MonoBehaviour
{
    [SerializeField] private GameObject DgGenerator;
    
    void Start()
    {
        Instantiate(DgGenerator);
    }

    void Update()
    {
        
    }
}
