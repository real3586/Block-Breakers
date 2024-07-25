using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserOverload : Laser
{
    protected override void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        rotationSpeed = baseRotation;
        laserDelay = baseDelay;
        laserReloadDelay = baseReload;

        reloadingText = GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>();
        reloadingText.gameObject.SetActive(false);

        sparks = GameObject.Find("Laser Sparks").GetComponent<ParticleSystem>();

        // Store the initial rotation of the laser
        initialRotation = transform.localRotation;
        continuousRotation = Quaternion.identity;

        // the only difference, set max ammo to 100
        LaserMaxRounds = 100;
        LaserRounds = 100;
        StartCoroutine(LaserCoroutine());
    }
}
