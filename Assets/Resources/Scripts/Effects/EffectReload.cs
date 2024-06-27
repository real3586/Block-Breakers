using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReload : Effect
{
    protected override IEnumerator ReturnToLaser()
    {
        yield return StartCoroutine(base.ReturnToLaser());

        Laser.Instance.LaserRounds = 30;
        Destroy(gameObject);
        yield return null;
    }
}
