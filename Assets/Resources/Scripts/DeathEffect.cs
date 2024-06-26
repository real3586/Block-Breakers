using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    ParticleSystem system;
    public GameObject parent;

    private void OnEnable()
    {
        system = GetComponent<ParticleSystem>();
        var mainSystem = system.main;
        mainSystem.startColor = parent.GetComponent<Renderer>().material.color;

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForEndOfFrame();
        system.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
