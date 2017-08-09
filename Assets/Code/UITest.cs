using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITest : MonoBehaviour
{
    private Text txt;
    private Slider slider;
    private Button compressBtn;
    private Button workBtn;

    void Awake()
    {
        slider = this.transform.FindChild("Slider").GetComponent<Slider>();
        txt = this.transform.FindChild("Text").GetComponent<Text>();
        
        compressBtn = this.transform.FindChild("Compress").GetComponent<Button>();
        compressBtn.onClick.AddListener(Compress);

        workBtn = this.transform.FindChild("Work").GetComponent<Button>();
        workBtn.onClick.AddListener(() =>
        {
            this.StartCoroutine(IE_Work());
        });
    }

    void Start()
    {
    }

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
        foreach (string file in TestCompressUtil.TestFiles)
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
        txt.text = "Finish";
    }

    #endregion MultiCompress

    private IEnumerator IE_Work()
    {
        txt.text = "Ready";
        yield return this.StartCoroutine(IE_Load());
        yield return new WaitForSeconds(0.2f);
        yield return this.StartCoroutine(IE_Decompress());
        yield return new WaitForSeconds(0.2f);
        txt.text = "Finish";
    }

    private IEnumerator IE_Load()
    {
        int tar = 50;
        float rate = 0;
        float stime = Time.realtimeSinceStartup;
        for (int i = 0; i <= tar; i++)
        {
            rate = i * 1.0f / tar;
            txt.text = string.Format("Loading {0:G}/{1:G} {2:F2}% {3:F2}", i, tar, rate * 100, Time.realtimeSinceStartup - stime);
            slider.value = rate;
            if (i == tar)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator IE_Decompress()
    {
        int tar = 50;
        float rate = 0;
        float stime = Time.realtimeSinceStartup;
        for (int i = 0; i <= tar; i++)
        {
            rate = i * 1.0f / tar;
            txt.text = string.Format("Decompressing {0:G}/{1:G} {2:F2}% {3:F2}", i, tar, rate * 100, Time.realtimeSinceStartup - stime);
            slider.value = rate;
            if (i == tar)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}