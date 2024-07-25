using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReload : Effect
{
    protected override IEnumerator DeathEffect()
    {
        yield return StartCoroutine(base.DeathEffect());

        Laser.Instance.LaserRounds = Laser.Instance.LaserMaxRounds;
        Destroy(gameObject);
        yield return null;
    }
}
