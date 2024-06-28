using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurbo : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.Turbo;
    }
}
