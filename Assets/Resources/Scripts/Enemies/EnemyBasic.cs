using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.Basic;
        PreferedEra = Enums.Era.Early;
    }
}
