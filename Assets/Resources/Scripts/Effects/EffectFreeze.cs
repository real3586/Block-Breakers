using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFreeze : Effect
{
    new static float startTime;

    protected override IEnumerator DeathEffect()
    {
        yield return StartCoroutine(base.DeathEffect());

        // place a global 50% freeze on all enemies for 3 seconds
        if (GameManager.Instance.enemySpeedMult == 1)
        {
            GameManager.Instance.enemySpeedMult = 0.5f;
        }
        startTime = Time.time;

        while (Time.time - startTime < 3)
        {
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.enemySpeedMult = 1;
        Destroy(gameObject);
    }
}
