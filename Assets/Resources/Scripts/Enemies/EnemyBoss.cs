using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    private void Awake()
    {
        Type = Enums.Enemies.Boss;
        PreferedEra = Enums.Era.Early;
    }
}
