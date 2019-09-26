using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンの自動生成モジュール
/// </summary>
// 参照元:https://qiita.com/2dgames_jp/items/00ee2ad52914753bfbb7
public class DgGenerator : MonoBehaviour
{
    /// <summary>
    /// 2次元配列情報
    /// </summary>
    Layer2D _layer = null;
    /// <summary>
    /// 区画リスト
    /// </summary>
    List<DgDivision> _divList = null;
}
