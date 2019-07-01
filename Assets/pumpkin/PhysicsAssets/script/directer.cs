using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class directer : MonoBehaviour
{
    /// <summary>
    /// batPrefabオブジェクト
    /// </summary>
    [SerializeField] private GameObject batPrefab;
    /// <summary>
    /// batオブジェクトのスクリプト
    /// </summary>
    private batController batController;
    /// <summary>
    /// fireオブジェクトのスクリプト
    /// </summary>
    private fireController fireController;
    /// <summary>
    /// firePrefabオブジェクト
    /// </summary>
    [SerializeField] private GameObject firePrefab;
    /// <summary>
    /// インスタンスしたbatオブジェクト
    /// </summary>
    private GameObject bat;
    /// <summary>
    /// インスタンスしたfireオブジェクト
    /// </summary>
    private GameObject fire;
    /// <summary>
    /// batオブジェクトのポジション
    /// </summary>
    [SerializeField] private Vector2 bPosition;
    /// <summary>
    /// fireオブジェクトのポジション
    /// </summary>
    [SerializeField] private Vector2 fPosition;

    void Start()
    {
        bat = Instantiate(batPrefab, bPosition, Quaternion.identity);
        fire = Instantiate(firePrefab, fPosition, Quaternion.identity);
        fireController = fire.GetComponent<fireController>();
        batController = bat.GetComponent<batController>();
    }

    // Update is called once per frame
    void Update()
    {
        fireController.bPosition = bat.transform.position;
        fireController.targetSpeed = batController.speed;
        if (Input.touchCount > 0)
        {
            batController.touch.x = Input.GetTouch(0).position.x;//スクリーン座標を記録
            batController.touch.y = Input.GetTouch(0).position.y;//スクリーン座標を記録
            batController.touch = Camera.main.ScreenToWorldPoint(batController.touch);//スクリーン座標をワールド座標に変換
        }
        if (Input.GetMouseButton(0))//開発用、ビルド時コメントアウト
        {
            batController.touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount >= 3)
        {
            SceneManager.LoadScene("Flocking");
        }
    }
}
