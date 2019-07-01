using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class directer_flocks : MonoBehaviour
{
    /// <summary>
    /// eyeオブジェクト
    /// </summary>
    [SerializeField] private GameObject eye;
    /// <summary>
    /// eyeオブジェクトの配列
    /// </summary>
    private GameObject[] eyes;
    /// <summary>
    /// eyeオブジェクトのスクリプトの配列
    /// </summary>
    private eye_flocks[] eye_Flocks;
    /// <summary>
    /// eye配列の要素数
    /// </summary>
    [SerializeField] private int elements;
    /// <summary>
    /// eyeオブジェクトから別のeyeオブジェクトへの距離ベクトル
    /// </summary>
    private Vector2 target;
    /// <summary>
    /// 方向ベクトルと距離ベクトルの角度差
    /// </summary>
    private float deltaAngle;
    /// <summary>
    /// stoneプレファブ
    /// </summary>
    [SerializeField] private GameObject stone;
    /// <summary>
    /// stoneオブジェクトの配列
    /// </summary>
    private GameObject[] stones;
    /// <summary>
    /// stoneの数
    /// </summary>
    [SerializeField] private int stoneElements;

    void Start()
    {
        eyes = new GameObject[elements];
        eye_Flocks = new eye_flocks[elements];
        stones = new GameObject[stoneElements];
        for(int n = 0; n <= stoneElements - 1; n++)
        {
            stones[n] = Instantiate(stone);
        }
        for(int i = 0; i <= elements - 1; i++)
        {
            eyes[i] = Instantiate(eye);//プレファブを生成
            eye_Flocks[i] = eyes[i].GetComponent<eye_flocks>();
            eye_Flocks[i].member = new GameObject[elements - 1];
            eye_Flocks[i].directions = new Vector2[elements - 1];
            eye_Flocks[i].distance = new Vector2[elements - 1];
            eye_Flocks[i].stone = new GameObject[stoneElements];
            for (int m = 0; m <= stoneElements - 1; m++)
            {
                eye_Flocks[i].stone[m] = stones[m];//stoneオブジェクトを格納
            }
        }
    }

    void Update()
    {
        for (int i = 0; i <= elements - 1; i++)
        {
            if (Input.touchCount > 0)
            {
                eye_Flocks[i].touch.x = Input.GetTouch(0).position.x;//スクリーン座標を記録
                eye_Flocks[i].touch.y = Input.GetTouch(0).position.y;//スクリーン座標を記録
                eye_Flocks[i].touch = Camera.main.ScreenToWorldPoint(eye_Flocks[i].touch);//スクリーン座標をワールド座標に変換
            }
            if (Input.GetMouseButton(0))//開発用、ビルド時コメントアウト
            {
                eye_Flocks[i].touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            for (int n = 0; n <= elements - 1; n++)
            {
                if (eye_Flocks[i] == eye_Flocks[n])
                {
                    continue;
                }
                target = eyes[i].transform.position - eyes[n].transform.position;
                deltaAngle = Mathf.Abs(Vector3.Angle(eye_Flocks[i].direction.normalized, target.normalized));
                if (Mathf.Abs((target).magnitude) <= eye_Flocks[i].viewLength //視界距離内にオブジェクトが存在する
                    && deltaAngle <= eye_Flocks[i].viewAngle / 2)//視界角度内にオブジェクトが存在する
                {
                    eye_Flocks[i].member[eye_Flocks[i].number] = eyes[n];//条件を満たしたオブジェクトを配列に格納する
                    eye_Flocks[i].directions[eye_Flocks[i].number] = eye_Flocks[n].direction;//条件を満たしたオブジェクトの方向ベクトルを配列に格納する
                    eye_Flocks[i].distance[eye_Flocks[i].number] = target;
                    eye_Flocks[i].number++;//要素番号を更新
                    eye_Flocks[i].sNumber = stoneElements;//障害物の数を格納
                }
            }
            for(int a= eye_Flocks[i].number; a <= elements - 2; a++)
            {
                eye_Flocks[i].member[a] = null;//初期化
                eye_Flocks[i].directions[a] = Vector2.zero;
            }
        }
        if (Input.touchCount >= 3)
        {
            SceneManager.LoadScene("PredetorScene");
        }
    }
}
