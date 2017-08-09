using System.Collections.Generic;
using UnityEngine;

public class TestCompressUtil
{
    public static List<string> TestFiles
    {
        get
        {
            return new List<string>()
            {
                "general_assets_bundle",
                "pet005_bundle",
            };
        }

    }

    public static string OringinDir
    {
        get
        {
            return Application.dataPath + "/../OriginAssets/Android/";
        }
    }

    public static string StreamingAssets
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    public static string PersistentDataPath
    {
        get
        {
            return Application.dataPath + "/PersistentDataPath/";
        }
    }
}