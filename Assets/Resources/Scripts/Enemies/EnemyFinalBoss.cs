using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinalBoss : Enemy
{
    private void OnEnable()
    {
        type = Enums.Enemies.FinalBoss;
        preferedEra = Enums.Era.End;
    }

    protected override void MoveToExit()
    {
        Quaternion routeAngle = Quaternion.Euler(0, Angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, routeAngle, 9999);

        // the final boss is unaffected by the chill multiplier
        transform.position += speed * GameManager.Instance.generalSpeedMultiplier 
            * Time.deltaTime * transform.forward;

        if (transform.position.z <= -0.5f)
        {
            GameManager.Instance.PlayerHealth -= health;
            Death();
        }
    }
}
