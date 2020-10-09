using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ABCreator : Editor {

    [MenuItem("Assets/Build Asset Bundles ")]
    static void BuildAll()
    {
        BuildPipeline.BuildAssetBundles("C:/Users/Ryan/AssetBundle", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }
}
