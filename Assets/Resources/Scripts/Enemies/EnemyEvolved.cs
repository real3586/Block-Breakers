using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvolved : Enemy
{
    const float baseSpeed = 0.5f, baseHealth = 750, baseScale = 0.75f;
    float evolvedMultiplier;

    private void OnEnable()
    {
        // when it gets spawned in, change the stats to reflect the number of evolveds that have been spawned
        Type = Enums.Enemies.Evolved;
        PreferedEra = Enums.Era.End;
        float evolvedCount = GameManager.Instance.evolvedEnemyCount > 20 ? 20 : GameManager.Instance.evolvedEnemyCount;
        evolvedMultiplier = evolvedCount / 10 + 1;

        speed = baseSpeed * evolvedMultiplier;
        health = (int)(baseHealth * evolvedMultiplier);
        maxHealth = health;

        float newScale = 0.0375f * evolvedCount + baseScale;
        transform.localScale = Vector3.one * newScale;

        // then increment the number
        GameManager.Instance.evolvedEnemyCount++;
    }
}
