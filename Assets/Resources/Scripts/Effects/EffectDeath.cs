using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeath : MonoBehaviour
{
    ParticleSystem system;
    Renderer render;
    GameObject parent;

    private void OnEnable()
    {
        parent = transform.parent.gameObject;
        system = GetComponent<ParticleSystem>();
        render = GetComponent<Renderer>();

        render.material = parent.GetComponent<Renderer>().material;
        var mainSystem = system.main;
        mainSystem.startSize = parent.transform.localScale.x / 4;

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
