using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFireRate : Effect
{
    protected override IEnumerator DeathEffect()
    {
        yield return StartCoroutine(base.DeathEffect());

        Laser.Instance.EffectFireRate();
        Destroy(gameObject);
        yield return null;
    }
}
