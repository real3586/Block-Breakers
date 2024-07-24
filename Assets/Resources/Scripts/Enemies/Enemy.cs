using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Enums.Enemies type;
    public Enums.Era preferedEra;    
    public float firstSpawnTime;

    public float speed;
    public int health;
    public int maxHealth;

    public int spawnScore;
    public int defeatScore;

    public float Angle { protected get; set; }

    public GameObject deathEffect;

    private void Awake()
    {
        maxHealth = health;
    }

    public virtual void DealDamage(int amount)
    {
        health -= amount;
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        // only for the handbook manager
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            float xPos = transform.position.x;
            float zPos = transform.position.z;

            // out of bound safeguards
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

    protected virtual void MoveToExit()
    {
        Quaternion routeAngle = Quaternion.Euler(0, Angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, routeAngle, 9999);

        transform.position += speed * GameManager.Instance.chillMultiplier 
            * GameManager.Instance.generalSpeedMultiplier * Time.deltaTime * transform.forward;

        if (transform.position.z <= -0.5f)
        {
            GameManager.Instance.PlayerHealth -= health;
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bumper"))
        {
            Angle = 360 - Angle;
        }
    }

    protected virtual void Death()
    {
        deathEffect.SetActive(true);
        transform.DetachChildren();

        GameManager.Instance.PlayerScore += defeatScore;

        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.laserTargetingEnabled)
        {
            // if this is not the target, make it
            if (Laser.Instance.target != gameObject)
            {
                Laser.Instance.target = gameObject;
                TargetingIndicator.Instance.SummonIndicator(gameObject);
            }
            // if it is, disengage targeting
            else
            {
                Laser.Instance.target = null;
            }
        }
    }
}
