using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlow : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.Slow;
        preferedEra = Enums.Era.Early;
    }
}
