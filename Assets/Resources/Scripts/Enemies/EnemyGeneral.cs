using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneral : Enemy
{
    private void OnEnable()
    {
        Type = Enums.Enemies.General;
        PreferedEra = Enums.Era.End;
    }

    protected override void MoveToExit()
    {
        Quaternion routeAngle = Quaternion.Euler(0, Angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, routeAngle, 9999);

        // the only change is to not include the general speed multiplier here
        transform.position += speed * GameManager.Instance.chillMultiplier
            * Time.deltaTime * transform.forward;

        if (transform.position.z <= -0.5f)
        {
            GameManager.Instance.PlayerHealth -= health;
            Death();
        }
    }
}
