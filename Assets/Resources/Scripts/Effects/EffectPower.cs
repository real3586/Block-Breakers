using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPower : Effect
{
    protected override IEnumerator ReturnToLaser()
    {
        yield return StartCoroutine(base.ReturnToLaser());

        Beam.Instance.LaserDamage++;
        Destroy(gameObject);
        yield return null;
    }
}
