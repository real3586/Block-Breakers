using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRampager : Laser
{
    int index = 0;

    protected override void Awake()
    {
        Instance = this;
    }

    protected override IEnumerator LaserCoroutine()
    {
        index++;
        // every 5th hit will deal double damage
        if (index % 5 == 0)
        {
            Beam.Instance.DamageMultiplier = 2;
            laserBeam.transform.localScale = new Vector3(0.3f, 0.3f, 10);
        }
        laserBeam.SetActive(true);
        yield return new WaitForSeconds(laserDelay);
        
        laserBeam.SetActive(false);
        Beam.Instance.DamageMultiplier = 1;        
        laserBeam.transform.localScale = new Vector3(0.15f, 0.15f, 10);
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
}
