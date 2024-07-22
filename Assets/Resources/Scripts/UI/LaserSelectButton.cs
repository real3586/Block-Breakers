using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserSelectButton : MonoBehaviour
{
    [SerializeField] Enums.LaserEffects laserEffect;
    public void OnClick()
    {
        GameManager.Instance.currentLaserEffect = laserEffect;
        SceneManager.LoadScene("Main");
    }
}
