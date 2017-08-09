using System.Collections.Generic;
using System.IO;
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
                "ui_bundle",
                "pet002_bundle",
            };
        }
    }

    public static List<string> OriginFileNames
    {
        get
        {
            List<string> files = EditorUtil.GetFiles(new List<string>()
            {
                TestCompressUtil.OringinDir,
            });
            List<string> ret = new List<string>();
            foreach (string file in files)
            {
                ret.Add(Path.GetFileName(file));
            }
            return ret;
        }
    }

    public static string OringinDir
    {
        get
        {
            return Application.dataPath + "/../OriginAssets/Android/";
        }
    }

    public static string ZipDir
    {
        get
        {
            return Application.dataPath + "/../OriginAssets/Android2/";
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