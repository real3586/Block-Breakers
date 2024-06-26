using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    RaycastHit hitInfo = new();
    [SerializeField] float rotationSpeed = 250;
    [SerializeField] float laserDelay = 0.15f;

    private Quaternion initialRotation, continuousRotation;

    [SerializeField] GameObject laserBeam;
    [SerializeField] ParticleSystem sparks;

    void Start()
    {
        // Store the initial rotation of the laser
        initialRotation = transform.localRotation;
        continuousRotation = Quaternion.identity;

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

        // laser fire rate increases with health lost
        // 0.15 seconds at 100 hp, 0.05 seconds at 20 (for nice numbers)
        laserDelay = 0.00125f * GameManager.Instance.PlayerHealth + 0.025f;
    }

    IEnumerator LaserCoroutine()
    {
        laserBeam.SetActive(false);
        yield return new WaitForSeconds(laserDelay);
        laserBeam.SetActive(true);
        yield return new WaitForSeconds(laserDelay);
        StartCoroutine(LaserCoroutine());
    }
}
