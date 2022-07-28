using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabCooker : MonoBehaviour
{
    [Header("Layout")]
    [Range(10, 999)]
    public float height;
    [Range(10, 999)]
    public float width;
    public Image normalBar;
    public Image doubleBar;
    RectTransform normalRT;
    RectTransform doubleRT;

    float wholeBarHeight;

    [Header("stats")]
    [Range(0, 100)]
    [Tooltip("Percentage to get double")]
    public float doubleRate;
    [Range(0, 100)]
    [Tooltip("Percentage to get normal")]
    public float successRate;
    [Range(0.1f, 100)]
    [Tooltip("Speed of pointer")]
    public float pointerSpeed;

    [Header("Config")]
    Slider slider;
    bool isStop = false;
    bool goingup = true;
    float value;

    // Start is called before the first frame update
    void Start()
    {
        wholeBarHeight = GetComponent<RectTransform>().sizeDelta.y;
        slider = GetComponent<Slider>();
        normalRT = normalBar.GetComponent<RectTransform>();
        doubleRT = doubleBar.GetComponent<RectTransform>();
        float h = GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log("h = " + h);
        setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStop)
        {
            float up = goingup ? 1 : -1;
            value += Time.deltaTime * up * pointerSpeed;
            slider.value = value;
            if (slider.value == slider.maxValue || slider.value == slider.minValue)
            {
                value = slider.value;
                goingup = !goingup;
            }
        }
    }

    public void stop()
    {
        if (!isStop)
        {
            isStop = true;
            float val = slider.value;
            val = (slider.maxValue - slider.minValue) * 0.5f - slider.value;
            val = Mathf.Abs(val);

            Debug.Log(val);
            if (val <= doubleRate * 0.5f)
            {
                Debug.Log("Double");
            }
            else if (val <= successRate * 0.5f)
            {
                Debug.Log("normal");
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }

    public void start_()
    {
        value = 0;
        goingup = true;
        isStop = false;
    }

    void setup()
    {
        normalRT.sizeDelta = new Vector2(width, successRate * wholeBarHeight * 0.01f);
        doubleRT.sizeDelta = new Vector2(width, doubleRate * wholeBarHeight * 0.01f);
    }
}
