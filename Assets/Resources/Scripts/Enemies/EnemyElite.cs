using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite : Enemy
{
    [SerializeField] float afterSpeed;
    [SerializeField] GameObject shield;
    [SerializeField] int damageReduction;

    private void OnEnable()
    {
        Type = Enums.Enemies.Elite;
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

    protected override void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        if (health <= maxHealth / 2)
        {
            Destroy(shield);
            speed = afterSpeed;
        }

        float xPos = transform.position.x;
        float zPos = transform.position.z;
        if (xPos <= -4 || xPos >= 4)
        {
            Destroy(gameObject);
        }
        else if (zPos <= -1 || zPos >= 10)
        {
            Destroy(gameObject);
        }

        MoveToExit();
    }
}
