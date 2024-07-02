using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.Basic;
        preferedEra = Enums.Era.Early;
    }
}
