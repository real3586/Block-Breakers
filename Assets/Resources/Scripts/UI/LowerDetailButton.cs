using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerDetailButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.lowDetailEnabled = !GameManager.Instance.lowDetailEnabled;
    }
}
