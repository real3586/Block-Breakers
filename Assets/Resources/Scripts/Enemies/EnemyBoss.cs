using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.Boss;
        preferedEra = Enums.Era.Early;
    }
}
