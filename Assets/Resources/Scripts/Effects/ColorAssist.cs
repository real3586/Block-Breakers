using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAssist : MonoBehaviour
{
    public GameObject parent;

    private void Update()
    {
        try
        {
            transform.SetPositionAndRotation(parent.transform.position + 2 * Vector3.up, Quaternion.Euler(90, 0, 0));
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}
