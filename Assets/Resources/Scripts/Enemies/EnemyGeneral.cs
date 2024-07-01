using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneral : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.General;
        PreferedEra = Enums.Era.End;
    }
}
