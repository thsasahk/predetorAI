using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    /// <summary>
    /// playerオブジェクト
    /// </summary>
    public GameObject player;
    /// <summary>
    /// dragonオブジェクト
    /// </summary>
    public GameObject dragon;

    [SerializeField] private Vector2 pPosition;

    [SerializeField] private Vector2 dPosition;

    /*
    [SerializeField] private float pPositionX;

    [SerializeField] private float pPositionY;

    [SerializeField] private float dPositionX;

    [SerializeField] private float dPositionY;
    */

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, pPosition, Quaternion.identity);
        Instantiate(dragon, dPosition, Quaternion.identity);
        /*
        Instantiate(player, new Vector2(pPositionX, pPositionY), Quaternion.identity);
        Instantiate(dragon, new Vector2(dPositionX, dPositionY), Quaternion.identity);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
