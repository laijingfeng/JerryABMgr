using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITest : MonoBehaviour
{
    private Text txt;
    private Slider slider;

    void Awake()
    {
        slider = this.transform.FindChild("Slider").GetComponent<Slider>();
        txt = this.transform.FindChild("Text").GetComponent<Text>();
    }

    void Start()
    {
        this.StartCoroutine(IE_Work());
    }

    private IEnumerator IE_Work()
    {
        txt.text = "Ready";
        yield return this.StartCoroutine(IE_Load());
        yield return new WaitForSeconds(0.2f);
        yield return this.StartCoroutine(IE_Compress());
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

    private IEnumerator IE_Compress()
    {
        int tar = 50;
        float rate = 0;
        float stime = Time.realtimeSinceStartup;
        for (int i = 0; i <= tar; i++)
        {
            rate = i * 1.0f / tar;
            txt.text = string.Format("Compressing {0:G}/{1:G} {2:F2}% {3:F2}", i, tar, rate * 100, Time.realtimeSinceStartup - stime);
            slider.value = rate;
            if (i == tar)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}