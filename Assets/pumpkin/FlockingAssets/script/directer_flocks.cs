using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        eyes = new GameObject[elements];
        eye_Flocks = new eye_flocks[elements];
        for(int i = 0; i <= elements - 1; i++)
        {
            eyes[i] = Instantiate(eye);//プレファブを生成
            eye_Flocks[i] = eyes[i].GetComponent<eye_flocks>();
            eye_Flocks[i].member = new GameObject[elements];
        }
    }

    void Update()
    {
        for (int i = 0; i <= elements - 1; i++)
        {
            for(int n = 0; n <= elements - 1; n++)
            {
                if (i == n)
                {
                    return;
                }
                target = eyes[i].transform.position - eyes[n].transform.position;
                deltaAngle = Mathf.Abs(Vector3.Angle(eye_Flocks[i].direction.normalized, target.normalized));
                if (Mathf.Abs((target).magnitude) <= eye_Flocks[i].viewLength //視界距離内にオブジェクトが存在する
                    && deltaAngle <= eye_Flocks[i].viewAngle / 2)//視界角度内にオブジェクトが存在する
                {
                    eye_Flocks[i].member[eye_Flocks[i].number] = eyes[n];//条件を満たしたオブジェクトを配列に格納する
                    eye_Flocks[i].number++;//要素番号を更新
                }
            }
        }
    }
}
