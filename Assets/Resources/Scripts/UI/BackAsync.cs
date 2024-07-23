using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAsync : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.UnloadSettingsScene();
    }
}
