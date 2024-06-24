using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    RaycastHit hitInfo = new();
    [SerializeField] float rotationSpeed = 250;

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
            // Get the direction to the hit point
            Vector3 directionToMouse = hitInfo.point - transform.position;
            directionToMouse.y = 0;

            // Calculate the new rotation to look at the mouse
            Quaternion lookRotation = Quaternion.LookRotation(directionToMouse);

            // Combine the initial rotation, look rotation, and continuous rotation
            Quaternion combinedRotation;
            if (hitInfo.point == null)
            {
                combinedRotation = initialRotation * continuousRotation;
            }
            else
            {
                combinedRotation = initialRotation * lookRotation * continuousRotation;
            }
            transform.localRotation = combinedRotation;
        }
    }

    IEnumerator LaserCoroutine()
    {
        laserBeam.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        laserBeam.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(LaserCoroutine());
    }
}
