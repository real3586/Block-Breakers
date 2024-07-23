using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = Laser.Instance.LaserMaxRounds;
    }

    void Update()
    {
        slider.value = Laser.Instance.LaserRounds;
    }
}
