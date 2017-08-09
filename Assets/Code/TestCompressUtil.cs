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
                "Android",
            };
        }
    }

    public static void OutputFileNames()
    {
        List<string> files = TestCompressUtil.OriginFileNames;
        string ret = "";
        foreach (string file in files)
        {
            ret += string.Format("\"{0}\",\n", file);
        }
        Debug.LogWarning(ret);
    }

    public static List<string> OriginFileNames
    {
        get
        {
            List<string> files = EditorUtil.GetFiles(new List<string>()
            {
                TestCompressUtil.TestOringinDir,
            });
            List<string> ret = new List<string>();
            foreach (string file in files)
            {
                ret.Add(Path.GetFileName(file));
            }
            return ret;
        }
    }

    public static string TestOringinDir
    {
        get
        {
            return Application.dataPath + "/../OriginAssets/Android/";
        }
    }

    public static string PersistentDataPath
    {
        get
        {
            return Application.persistentDataPath + "/";
        }
    }

    public static string StreamingAssets
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
}