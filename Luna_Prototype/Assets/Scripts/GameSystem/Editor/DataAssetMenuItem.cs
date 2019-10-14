using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ScriptableObjectUtility
{
    public static void CreateAsset<T>() where T : ScriptableObject// run if T is ''
    {
        var asset = ScriptableObject.CreateInstance<T>();
        ProjectWindowUtil.CreateAsset(asset, "New" + typeof(T).Name + ".asset");
    }
}

public class DataAssetMenuItem : MonoBehaviour
{
    [MenuItem("Assets/Create/Data Source/Json Data Source")]
    public static void CreateJsonDataSource()
    {
        ScriptableObjectUtility.CreateAsset<JsonDataSource>();

    }

}
