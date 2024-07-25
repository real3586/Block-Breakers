using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsTooltip : MonoBehaviour
{
    TooltipManager manager;
    [SerializeField] int index;

    private void Start()
    {
        manager = GameObject.Find("Tooltip Manager").GetComponent<TooltipManager>();
    }

    public void MouseOver()
    {
        manager.DisplayTooltip(index, transform);
    }

    public void MouseOut()
    {
        manager.ResetTooltips();
    }
}
