using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UITest : MonoBehaviour
{
    private Text txt;
    private Slider slider;
    private Button compressBtn;
    private Button workBtn;
    private Button downloadBtn;
    private Button decompressBtn;

    private bool work = false;
    private float workStartTime = 0;

    void Awake()
    {
        slider = this.transform.FindChild("Slider").GetComponent<Slider>();
        txt = this.transform.FindChild("Text").GetComponent<Text>();

        compressBtn = this.transform.FindChild("Compress").GetComponent<Button>();
        compressBtn.onClick.AddListener(() =>
        {
            Compress();
        });

        workBtn = this.transform.FindChild("Work").GetComponent<Button>();
        workBtn.onClick.AddListener(() =>
        {
            workStartTime = Time.realtimeSinceStartup;
            work = true;
            MultiDownload();
        });

        downloadBtn = this.transform.FindChild("Download").GetComponent<Button>();
        downloadBtn.onClick.AddListener(() =>
        {
            MultiDownload();
        });

        decompressBtn = this.transform.FindChild("Decompress").GetComponent<Button>();
        decompressBtn.onClick.AddListener(() =>
        {
            Decompress();
        });
    }

    void Start()
    {
        //TestCompressUtil.OutputFileNames();
    }

    private void AfterDecompress()
    {
        txt.text = (Time.realtimeSinceStartup - workStartTime).ToString("F2");
    }

    private void AfterDownload()
    {
        if (work)
        {
            work = false;
            Decompress();
        }
    }

    #region MultiDownload

    private MultiDownLoad multiDownload = null;
    private float multiDownloadStartTime = 0;

    private void MultiDownload()
    {
        string fromDir = "http://192.168.10.249/assetsbundle/Android/";
        string toDir = TestCompressUtil.PersistentDataPath + "AndroidZip/";
        CheckDir(toDir);
        Debug.LogWarning(toDir);

        multiDownload = new MultiDownLoad();
        foreach (string file in TestCompressUtil.TestFiles)
        {
            multiDownload.AddOneConfig(new DownLoadConfig()
            {
                url = fromDir + CompressUtil.GetCompressFileName(file),
                savePath = toDir + CompressUtil.GetCompressFileName(file),
                retryCnt = 0,
            });
        }

        multiDownload.SetCallback((status, curSize, totalSize, error) =>
        {
            float rate = curSize * 1.0f / totalSize;
            txt.text = string.Format("Decompressing {0:G}/{1:G} {2:F2}% {3:F2}", curSize, totalSize, rate * 100, Time.realtimeSinceStartup - multiDownloadStartTime);
            slider.value = rate;
        });
        multiDownloadStartTime = Time.realtimeSinceStartup;
        multiDownload.StartDownLoad();
        this.StartCoroutine("IE_UpdateMultiDownLoad");
    }

    private IEnumerator IE_UpdateMultiDownLoad()
    {
        while (multiDownload != null)
        {
            multiDownload.UpdateCallback();
            if (multiDownload.Status != HTTPDownLoad.DownLoadStatus.DownLoading)
            {
                multiDownload.UpdateCallback();
                break;
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        txt.text = "Finish " + (Time.realtimeSinceStartup - multiDownloadStartTime).ToString("F2");
        AfterDownload();
    }

    #endregion MultiDownload

    #region MultiDecompress

    private MultiDecompress multiDecompress = null;
    private float multiDecompressStartTime = 0;

    private void Decompress()
    {
        multiDecompress = new MultiDecompress();
        multiDecompress.SetCallback((finish, total, status) =>
        {
            float rate = finish * 1.0f / total;
            txt.text = string.Format("Decompressing {0:G}/{1:G} {2:F2}% {3:F2}", finish, total, rate * 100, Time.realtimeSinceStartup - multiDecompressStartTime);
            slider.value = rate;
        });
        string inDir = TestCompressUtil.PersistentDataPath + "AndroidZip/";
        string outDir = TestCompressUtil.PersistentDataPath + "Android/";
        CheckDir(inDir);
        CheckDir(outDir);
        foreach (string file in TestCompressUtil.TestFiles)
        {
            multiDecompress.AddCompressConfig(new CompressConfig()
            {
                inFile = inDir + CompressUtil.GetCompressFileName(file),
                outFile = outDir + file,
            });
        }
        multiDecompressStartTime = Time.realtimeSinceStartup;
        multiDecompress.Start();
        this.StartCoroutine("IE_UpdateMultiDecompress");
    }

    private IEnumerator IE_UpdateMultiDecompress()
    {
        while (multiDecompress != null)
        {
            multiDecompress.UpdateCallback();
            if (multiDecompress.Status == CompressState.Finish)
            {
                multiDecompress.UpdateCallback();
                break;
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        txt.text = "Finish " + (Time.realtimeSinceStartup - multiCompressStartTime).ToString("F2");
        AfterDecompress();
    }

    #endregion MultiDecompress

    #region MultiCompress

    private MultiCompress multiCompress = null;
    private float multiCompressStartTime = 0;

    private void Compress()
    {
        multiCompress = new MultiCompress();
        multiCompress.SetCallback((finish, total, status) =>
        {
            float rate = finish * 1.0f / total;
            txt.text = string.Format("Compressing {0:G}/{1:G} {2:F2}% {3:F2}", finish, total, rate * 100, Time.realtimeSinceStartup - multiCompressStartTime);
            slider.value = rate;
        });
        foreach (string file in TestCompressUtil.OriginFileNames)
        {
            multiCompress.AddCompressConfig(new CompressConfig()
            {
                inFile = TestCompressUtil.TestOringinDir + file,
                outFile = TestCompressUtil.TestOringinDir + CompressUtil.GetCompressFileName(file),
            });
        }
        multiCompressStartTime = Time.realtimeSinceStartup;
        multiCompress.Start();
        this.StartCoroutine("IE_UpdateMultiCompress");
    }

    private IEnumerator IE_UpdateMultiCompress()
    {
        while (multiCompress != null)
        {
            multiCompress.UpdateCallback();
            if (multiCompress.Status == CompressState.Finish)
            {
                multiCompress.UpdateCallback();
                break;
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        txt.text = "Finish " + (Time.realtimeSinceStartup - multiCompressStartTime).ToString("F2");
    }

    #endregion MultiCompress

    private void CheckDir(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}