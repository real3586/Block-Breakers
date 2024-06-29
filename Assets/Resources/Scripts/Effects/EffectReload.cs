using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReload : Effect
{
    protected override IEnumerator DeathEffect()
    {
        yield return StartCoroutine(base.DeathEffect());

        Laser.Instance.LaserRounds = 30;
        Destroy(gameObject);
        yield return null;
    }
}
