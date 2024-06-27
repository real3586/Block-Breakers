using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Laser laser;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = laser.LaserMaxRounds;
    }

    void Update()
    {
        slider.value = laser.LaserRounds;
    }
}
