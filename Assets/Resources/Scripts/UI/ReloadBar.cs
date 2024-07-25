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
    }

    void Update()
    {        
        slider.maxValue = Laser.Instance.LaserMaxRounds;

        slider.value = Laser.Instance.LaserRounds;
    }
}
