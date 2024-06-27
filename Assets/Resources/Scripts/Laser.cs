using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public static Laser Instance { get; private set; }

    RaycastHit hitInfo = new();
    [SerializeField] float rotationSpeed = 250;
    [SerializeField] float laserDelay = 0.15f;
    [SerializeField] float laserReloadDelay = 1.5f;
    public int LaserMaxRounds { get; private set; } = 30;
    public int LaserRounds { get; set; }
    [SerializeField] TextMeshProUGUI reloadingText;

    private Quaternion initialRotation, continuousRotation;

    [SerializeField] GameObject laserBeam;
    [SerializeField] ParticleSystem sparks;

    private void Awake()
    {
        Instance = this;
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

        // laser fire rate increases with health lost (linearly)
        // 0.15 seconds at 100 hp, 0.075 seconds at 20 (for nice numbers)
        laserDelay = 0.0009375f * GameManager.Instance.PlayerHealth + 0.05625f;

        // so does laser reload speed
        laserReloadDelay = 0.009375f * GameManager.Instance.PlayerHealth + 0.5625f;

        // and rotation speed
        rotationSpeed = -3.125f * GameManager.Instance.PlayerHealth + 562.5f;

        // and the spark particle emission, because why not
        var sparksEmission = sparks.emission;
        sparksEmission.rateOverTime = -0.125f * GameManager.Instance.PlayerHealth + 17.5f;
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
            yield return new WaitForSeconds(laserReloadDelay);
            
            LaserRounds = LaserMaxRounds;
            reloadingText.gameObject.SetActive(false);
        }
        StartCoroutine(LaserCoroutine());
    }
}
