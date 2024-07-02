using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedy : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.Speedy;
        preferedEra = Enums.Era.Early;
    }
}
