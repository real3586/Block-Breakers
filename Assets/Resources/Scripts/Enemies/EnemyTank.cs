using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTank : Enemy
{
    [SerializeField] GameObject shield;
    [SerializeField] float afterSpeed;

    private void OnEnable()
    {
        type = Enums.Enemies.Tank;
        preferedEra = Enums.Era.Mid;
    }

    protected override void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        if (health <= maxHealth / 2)
        {
            Destroy(shield);
            speed = afterSpeed;
        }

        // only for the handbook manager
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            float xPos = transform.position.x;
            float zPos = transform.position.z;
            if (xPos <= -4 || xPos >= 4)
            {
                Destroy(gameObject);
            }
            else if (zPos <= -1 || zPos >= 10)
            {
                Destroy(gameObject);
            }

            MoveToExit();
        }
    }
}
