using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public static Beam Instance { get; private set; }

    public int LaserDamage { get; set; }
    public float DamageMultiplier { get; set; }

    private void Awake()
    {
        LaserDamage = 1;
        DamageMultiplier = 1;
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy otherEnemy = other.GetComponent<Enemy>();
            
            switch (GameManager.Instance.currentLaserEffect)
            {
                case Enums.Lasers.Execute:
                    float a = otherEnemy.health, b = otherEnemy.maxHealth;
                    if (a / b <= 0.5f || Mathf.Approximately(a / b, 0.5f))
                    {
                        otherEnemy.DealDamage(Mathf.RoundToInt(LaserDamage * 1.25f));
                    }
                    else
                    {
                        otherEnemy.DealDamage(Mathf.RoundToInt(LaserDamage));
                    }
                    break;

                case Enums.Lasers.Shatter:
                    if (otherEnemy.health == otherEnemy.maxHealth)
                    {
                        otherEnemy.DealDamage(Mathf.RoundToInt(LaserDamage * 3));
                    }
                    else
                    {
                        otherEnemy.DealDamage(Mathf.RoundToInt(LaserDamage));
                    }
                    break;

                default:            
                    otherEnemy.DealDamage(Mathf.RoundToInt(LaserDamage * DamageMultiplier));
                    break;
            }
        }
    }
}
