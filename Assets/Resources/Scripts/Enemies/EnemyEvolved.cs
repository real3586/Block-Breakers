using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEvolved : Enemy
{
    const float baseSpeed = 0.5f, baseHealth = 725, baseScale = 0.75f;
    float evolvedMultiplier;

    private void OnEnable()
    {
        // when it gets spawned in, change the stats to reflect the number of evolveds that have been spawned
        type = Enums.Enemies.Evolved;
        preferedEra = Enums.Era.End;

        // only for the handbook manager
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
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
}
