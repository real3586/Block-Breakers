using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDocHandbook : MonoBehaviour
{
    public void OnClick()
    {
        Application.OpenURL("https://docs.google.com/document/d/1Al_2jTBFHQ3vOeZTWQoE5uCqODnbh-HJ1W-r1H3weBs/edit?usp=sharing");
    }
}
