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

    void Awake()
    {
        slider = this.transform.FindChild("Slider").GetComponent<Slider>();
        txt = this.transform.FindChild("Text").GetComponent<Text>();

        compressBtn = this.transform.FindChild("Compress").GetComponent<Button>();
        compressBtn.onClick.AddListener(Compress);

        workBtn = this.transform.FindChild("Work").GetComponent<Button>();
        workBtn.onClick.AddListener(() =>
        {
        });

        downloadBtn = this.transform.FindChild("Download").GetComponent<Button>();
        downloadBtn.onClick.AddListener(() =>
        {
            this.StartCoroutine(IE_Load());
        });

        decompressBtn = this.transform.FindChild("Decompress").GetComponent<Button>();
        decompressBtn.onClick.AddListener(() =>
        {
            Decompress();
        });
    }

    void Start()
    {
    }

    private void AfterDecompress()
    {
    }

    private void AfterDownload()
    {
    }

    #region Download

    private IEnumerator IE_Load()
    {
        int tar = 50;
        float rate = 0;
        float stime = Time.realtimeSinceStartup;
        for (int i = 0; i <= tar; i++)
        {
            rate = i * 1.0f / tar;
            txt.text = string.Format("Downloading {0:G}/{1:G} {2:F2}% {3:F2}", i, tar, rate * 100, Time.realtimeSinceStartup - stime);
            slider.value = rate;
            if (i == tar)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        AfterDownload();
    }

    #endregion Download

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
        foreach (string file in TestCompressUtil.OriginFileNames)
        {
            multiDecompress.AddCompressConfig(new CompressConfig()
            {
                inFile = TestCompressUtil.ZipDir + CompressUtil.GetCompressFileName(file),
                outFile = TestCompressUtil.PersistentDataPath + file,
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
                inFile = TestCompressUtil.OringinDir + file,
                outFile = TestCompressUtil.OringinDir + CompressUtil.GetCompressFileName(file),
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
}