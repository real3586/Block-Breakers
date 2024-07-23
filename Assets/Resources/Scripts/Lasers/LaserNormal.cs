using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserNormal : Laser
{
    protected override void Awake()
    {
        Instance = this;
    }
}
