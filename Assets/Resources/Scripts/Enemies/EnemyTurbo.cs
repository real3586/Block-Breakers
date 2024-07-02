using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurbo : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.Turbo;
        preferedEra = Enums.Era.Mid;
    }
}
