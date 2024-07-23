using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeScaleButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scaleText;
    float[] scales = new float[] { 0.5f, 1f, 2f };
    int index = 1;

    public void OnClick()
    {
        index++;
        if (index >= scales.Length)
        {
            index = 0;
        }
        Time.timeScale = scales[index];
    }
    
    public void ResetIndex()
    {
        index = 1;
    }

    private void Update()
    {
        switch (index)
        {
            case 0:
                scaleText.text = "0.5x";
                break;
            case 1:
                scaleText.text = "1x";
                break;
            case 2:
                scaleText.text = "2x";
                break;
        }
    }
}
