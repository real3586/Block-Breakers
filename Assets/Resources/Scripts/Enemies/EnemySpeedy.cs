using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedy : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.Speedy;
        PreferedEra = Enums.Era.Early;
    }
}
