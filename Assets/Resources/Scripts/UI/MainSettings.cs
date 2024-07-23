using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSettings : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.LoadSettingsScene();
    }
}
