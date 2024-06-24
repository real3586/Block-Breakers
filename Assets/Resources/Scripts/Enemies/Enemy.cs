using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth;

    private void Awake()
    {
        maxHealth = health;
        gameObject.tag = "Enemy";
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
    }
}
