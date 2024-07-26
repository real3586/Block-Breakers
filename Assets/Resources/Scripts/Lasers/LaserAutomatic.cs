using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LaserAutomatic : Laser
{
    // replaces base delay because i want it to be faster
    const float baseDelayAutomatic = 0.135f;

    protected override void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        rotationSpeed = baseRotation;
        laserDelay = baseDelayAutomatic;
        laserReloadDelay = baseReload;

        reloadingText = GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>();
        reloadingText.gameObject.SetActive(false);

        sparks = GameObject.Find("Laser Sparks").GetComponent<ParticleSystem>();

        // Store the initial rotation of the laser
        initialRotation = transform.localRotation;
        continuousRotation = Quaternion.identity;

        // lol infinite ammo
        LaserMaxRounds = 1000;
        LaserRounds = LaserMaxRounds;
        StartCoroutine(LaserCoroutine());
    }

    protected override void Update()
    {
        // most of this code is reused
        // but this laser will not take mouse input :)

        // continuous rotation for the cool effect
        continuousRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

        if (Time.timeScale != 0)
        {
            Quaternion combinedRotation;

            // aim at the target
            target = SearchForTarget();

            // if the laser is active, don't move or rotate
            if (!laserBeam.activeSelf && target != null)
            {

                // Get the direction to the enemy
                Vector3 directionToMouse = target.transform.position - transform.position;
                directionToMouse.y = 0;

                // Calculate the new rotation to look at the mouse
                Quaternion lookRotation = Quaternion.LookRotation(directionToMouse);

                // Combine the initial rotation, look rotation, and continuous rotation
                combinedRotation = initialRotation * lookRotation * continuousRotation;
            }
            else
            {
                // Combine the look rotation, and continuous rotation
                Quaternion currentRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

                combinedRotation = initialRotation * currentRotation * continuousRotation;
            }
            transform.localRotation = combinedRotation;
        }

        // laser reload speed increases with fire rate
        // as fire rate approaches 0, reload approaches 0.25
        laserReloadDelay = 0.25f * Mathf.Pow(10321.3f, laserDelay);

        // and rotation speed
        // approaches 500 as fire rate approaches 0
        rotationSpeed = 1000 * Mathf.Pow(0.0000968f, laserDelay) * GameManager.Instance.detailMultiplier;

        // and the spark particle emission, because why not
        // approaches 10 as fire rate approaches 0
        // base emission is 5
        var sparksEmission = sparks.emission;
        sparksEmission.rateOverTime = 20 * Mathf.Pow(0.0000968f, laserDelay) * GameManager.Instance.detailMultiplier;
    }

    private GameObject SearchForTarget()
    {
        List<GameObject> targets = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        if (targets.Count == 0)
        {
            return null;
        }

        float minDistance = 1000;
        int index = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            float distance = Vector3.Distance(Vector3.zero, targets[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }
        return targets[index];
    }
}
