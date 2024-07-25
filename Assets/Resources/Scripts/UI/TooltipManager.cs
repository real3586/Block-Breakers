using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tooltipText;
    [SerializeField] GameObject panel;

    List<string> tooltips = new();

    float xLock;

    private void Start()
    {
        tooltips.Clear();
        tooltips.Add("Shows letters to help identify the effects.");
        tooltips.Add("Halves the amount of particles in game.");
        tooltips.Add("Click on enemies for your laser to target them.");

        xLock = transform.position.x;
        ResetTooltips();
    }

    public void DisplayTooltip(int index, Transform pos)
    {
        transform.position = new Vector3(xLock, pos.position.y, pos.position.z);

        tooltipText.gameObject.SetActive(true);
        panel.SetActive(true);

        tooltipText.text = tooltips[index];
    }

    public void ResetTooltips()
    {
        tooltipText.gameObject.SetActive(false);
        panel.SetActive(false);
    }
}
