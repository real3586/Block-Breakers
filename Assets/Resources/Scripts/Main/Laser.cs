using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public static Laser Instance { get; private set; }

    RaycastHit hitInfo = new();

    const float baseRotation = 250, baseDelay = 0.15f, baseReload = 1f;
    [SerializeField] float rotationSpeed = 250;
    [SerializeField] float laserDelay = 0.15f;
    [SerializeField] float laserReloadDelay = 1f;
    public int LaserMaxRounds { get; private set; } = 30;
    public int LaserRounds { get; set; }
    [SerializeField] TextMeshProUGUI reloadingText;

    private Quaternion initialRotation, continuousRotation;

    [SerializeField] GameObject laserBeam;
    [SerializeField] ParticleSystem sparks;

    private void Awake()
    {
        Instance = this;

        rotationSpeed = baseRotation;
        laserDelay = baseDelay;
        laserReloadDelay = baseReload;
    }

    void Start()
    {
        // Store the initial rotation of the laser
        initialRotation = transform.localRotation;
        continuousRotation = Quaternion.identity;

        LaserRounds = 30;
        StartCoroutine(LaserCoroutine());
    }

    void Update()
    {
        // continuous rotation for the cool effect
        continuousRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

        // Raycast to find the point the mouse is pointing at
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Quaternion combinedRotation;
            // if the laser is active, don't move or rotate
            if (!laserBeam.activeSelf)
            {
                // Get the direction to the hit point
                Vector3 directionToMouse = hitInfo.point - transform.position;
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

    IEnumerator LaserCoroutine()
    {
        laserBeam.SetActive(true);
        yield return new WaitForSeconds(laserDelay);
        laserBeam.SetActive(false);
        yield return new WaitForSeconds(laserDelay);

        LaserRounds--;
        if (LaserRounds <= 0)
        {
            reloadingText.gameObject.SetActive(true);
            float startTime = Time.time;
            while (Time.time - startTime < laserReloadDelay)
            {
                if (LaserRounds > 0)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            LaserRounds = LaserMaxRounds;
            reloadingText.gameObject.SetActive(false);
        }
        StartCoroutine(LaserCoroutine());
    }

    public void EffectFireRate()
    {
        laserDelay *= 0.96f;
    }
}