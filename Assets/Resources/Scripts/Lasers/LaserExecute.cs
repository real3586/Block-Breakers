using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserExecute : Laser
{
    protected override void Awake()
    {
        Instance = this;
    }

    // for this laser all the functionality is inside the beam script
}
