using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    private void Awake()
    {
        Type = Enums.Enemies.Basic;
    }
}
