using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Laser : MonoBehaviour
{
    public static Laser Instance { get; protected set; }

    RaycastHit hitInfo = new();

    protected const float baseRotation = 250, baseDelay = 0.15f, baseReload = 1f;
    [SerializeField] protected float rotationSpeed = 250;
    [SerializeField] protected float laserDelay = 0.15f;
    [SerializeField] protected float laserReloadDelay = 1f;
    public int LaserMaxRounds { get; private set; } = 30;
    public int LaserRounds { get; set; }

    protected Quaternion initialRotation, continuousRotation;

    [SerializeField] protected GameObject laserBeam;
    protected ParticleSystem sparks;    
    protected TextMeshProUGUI reloadingText;

    public GameObject target;

    protected abstract void Awake();

    void Start()
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

        LaserRounds = 30;
        StartCoroutine(LaserCoroutine());
    }

    protected virtual void Update()
    {
        // continuous rotation for the cool effect
        continuousRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

        // Raycast to find the point the mouse is pointing at
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo) && Time.timeScale != 0)
        {
            Quaternion combinedRotation;
            // if the laser is active, don't move or rotate
            if (!laserBeam.activeSelf)
            {
                if (GameManager.Instance.laserTargetingEnabled && target != null)
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
                    // Get the direction to the hit point
                    Vector3 directionToMouse = hitInfo.point - transform.position;
                    directionToMouse.y = 0;

                    // Calculate the new rotation to look at the mouse
                    Quaternion lookRotation = Quaternion.LookRotation(directionToMouse);

                    // Combine the initial rotation, look rotation, and continuous rotation
                    combinedRotation = initialRotation * lookRotation * continuousRotation;
                }
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

    protected virtual IEnumerator LaserCoroutine()
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