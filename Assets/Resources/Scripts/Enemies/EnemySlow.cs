using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlow : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.Slow;
    }
}
