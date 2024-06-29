using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectExplosion : Effect
{
    public int explosionDamage;
    [SerializeField] float explosionRadius = 3;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] Renderer explosionRender;
    [SerializeField] Material explosionColor;

    const int defaultEmission = 250;

    protected override void OnEnable()
    {
        var mainExplosion = explosion.main;
        explosionRender.material = explosionColor;
        mainExplosion.startSize = transform.parent.localScale.x / 4;

        var emission = explosion.emission;
        emission.rateOverTime = defaultEmission * GameManager.Instance.detailMultiplier;

        StartCoroutine(FlashSequence());
        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        // wait until the enemy that this is attached to dies
        yield return new WaitUntil(() => transform.parent == null);

        if (transform.position.z <= -0.5f)
        {
            Destroy(gameObject);
            yield break;
        }

        light.enabled = false;
        explosion.gameObject.SetActive(true);
        explosion.Play();

        yield return new WaitForSeconds(0.1f);

        // do a calculation with vector3 distance
        List<GameObject> allEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        List<GameObject> enemiesToExplode = new();
        Vector3 compareDistance = new(transform.position.x, transform.position.y - 2, transform.position.z);
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (Vector3.Distance(compareDistance, allEnemies[i].transform.position) <= explosionRadius)
            {
                enemiesToExplode.Add(allEnemies[i]);
            }
        }

        // all enemies that explode take damage equal to the exploded enemy's max health
        // explosion damage is handled by game manager
        for (int i = 0; i < enemiesToExplode.Count; i++)
        {
            enemiesToExplode[i].GetComponent<Enemy>().DealDamage(explosionDamage);
        }

        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
