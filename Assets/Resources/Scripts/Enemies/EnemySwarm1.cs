using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public class EnemySwarm1 : Enemy
{
    [SerializeField] GameObject swarm2Prefab;

    private void OnEnable()
    {
        type = Enums.Enemies.Swarm;
        preferedEra = Enums.Era.Mid;
    }

    protected override void Death()
    {
        // some code from game manager
        // spawn two clones near its place when it dies
        for (int i = 0; i < 2; i++)
        {
            float angle = Rand.Range(1, 3) == 1 ? Rand.Range(110.0f, 150.0f) : Rand.Range(210.0f, 250.0f);
            GameObject newClone = Instantiate(swarm2Prefab, transform.position + (-2 * i + 1) * 0.25f * Vector3.right,
                Quaternion.identity, GameManager.Instance.allEnemies.transform);
            newClone.GetComponent<Enemy>().Angle = angle;
            newClone.tag = "Enemy";
        }
        base.Death();
    }
}
