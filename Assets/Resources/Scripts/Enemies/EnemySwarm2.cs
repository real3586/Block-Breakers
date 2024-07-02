using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarm2 : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.Swarm;
        preferedEra = Enums.Era.Mid;
    }
}
