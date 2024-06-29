using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAssistButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.colorAssistEnabled = !GameManager.Instance.colorAssistEnabled;
    }
}
