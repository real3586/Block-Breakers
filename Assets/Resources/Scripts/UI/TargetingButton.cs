using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.laserTargetingEnabled = !GameManager.Instance.laserTargetingEnabled;
    }
}
