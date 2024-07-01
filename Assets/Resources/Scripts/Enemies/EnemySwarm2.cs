using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarm2 : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.Swarm;
        PreferedEra = Enums.Era.Mid;
    }
}
