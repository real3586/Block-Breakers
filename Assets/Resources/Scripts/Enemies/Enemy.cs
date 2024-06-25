using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth;

    public int spawnScore;
    public int defeatScore;

    float angle;

    private void Awake()
    {
        maxHealth = health;
        gameObject.tag = "Enemy";

        // use the unit circle, quadrants 3 and 4, ignores the extreme 30 degrees
        // in Unity, 0 degrees is North, and rotation follows clockwise (like a compass)
        angle = Random.Range(120, 240);
    }

    public virtual void DealDamage(int amount)
    {
        health -= amount;
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        MoveToExit();
    }

    protected virtual void MoveToExit()
    {
        Quaternion routeAngle = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, routeAngle, 9999);

        transform.position += speed * Time.deltaTime * transform.forward;

        if (transform.position.z <= -0.5f)
        {
            GameManager.Instance.PlayerHealth -= health;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bumper"))
        {
            angle = 360 - angle;
        }
    }
}
