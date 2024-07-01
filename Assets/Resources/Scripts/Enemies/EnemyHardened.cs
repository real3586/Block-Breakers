using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHardened : Enemy
{
    [SerializeField] int damageReduction;

    private void OnEnable()
    {
        Type = Enums.Enemies.Hardened;
        PreferedEra = Enums.Era.Late;
    }

    public override void DealDamage(int amount)
    {
        if (amount - damageReduction > 0)
        {
            base.DealDamage(amount - damageReduction);
        }
        else
        {
            return;
        }
    }
}
