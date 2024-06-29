using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPower : Effect
{
    protected override IEnumerator DeathEffect()
    {
        yield return StartCoroutine(base.DeathEffect());

        Beam.Instance.LaserDamage++;
        Destroy(gameObject);
        yield return null;
    }
}
