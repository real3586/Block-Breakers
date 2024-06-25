using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Enums.Enemies Type { get; protected set; }

    public float speed;
    public int health;
    public int maxHealth;

    public int spawnScore;
    public int defeatScore;

    public float Angle { protected get; set; }

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
            Destroy(gameObject);
        }

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

    protected virtual void MoveToExit()
    {
        Quaternion routeAngle = Quaternion.Euler(0, Angle, 0);
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
            Angle = 360 - Angle;
        }
    }
}
