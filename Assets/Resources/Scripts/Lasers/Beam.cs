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
            other.GetComponent<Enemy>().DealDamage(Mathf.RoundToInt(LaserDamage * DamageMultiplier));
        }
    }
}
