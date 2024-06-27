using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] protected new Light light;
    [SerializeField] protected float delay;

    float startTime;

    private void OnEnable()
    {
        startTime = Time.time;

        StartCoroutine(FlashSequence());
        StartCoroutine(ReturnToLaser());
    }

    protected virtual IEnumerator FlashSequence()
    {
        float currentTime = Time.time - startTime;

        light.intensity = 1.5f * Mathf.Sin(currentTime * Mathf.PI * delay) + 2.5f;
        yield return new WaitForEndOfFrame();
        StartCoroutine(FlashSequence());
    }

    protected virtual IEnumerator ReturnToLaser()
    {
        // wait until the enemy that this is attached to dies
        yield return new WaitUntil(() => transform.parent == null);

        if (transform.position.z <= -0.5f)
        {
            Debug.Log("I died to the exit");
            Destroy(gameObject);
            yield break;
        }

        // start a lerp to the laser, which is at the origin
        Vector3 start = transform.position;
        float time = 0;
        while (Vector3.Distance(transform.position, Laser.Instance.transform.position) > 1)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(start, Laser.Instance.transform.position, time);
            yield return new WaitForEndOfFrame();
        }
    }
}
