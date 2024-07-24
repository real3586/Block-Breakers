using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetingIndicator : MonoBehaviour
{
    public static TargetingIndicator Instance { get; private set; }

    [SerializeField] TextMeshPro targetingIndicator;
    bool coroutineInProgress = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SummonIndicator(GameObject enemy)
    {
        // move to the enemy's position
        transform.position = enemy.transform.position;

        // scale up the indicator based on the size of the enemy
        targetingIndicator.fontSize = 28 / 3 * enemy.transform.localScale.x + 2.4f;

        StartCoroutine(FlashSequence());
    }

    IEnumerator FlashSequence()
    {
        coroutineInProgress = true;

        targetingIndicator.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);

        targetingIndicator.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.1f);

        targetingIndicator.gameObject.SetActive(true);

        coroutineInProgress= false;
    }

    private void Update()
    {
        if (Laser.Instance.target == null)
        {
            targetingIndicator.gameObject.SetActive(false);
        }
        else
        {
            if (!coroutineInProgress)
            {
                targetingIndicator.gameObject.SetActive(true);
            }
            transform.position = Laser.Instance.target.transform.position;
        }
    }
}
