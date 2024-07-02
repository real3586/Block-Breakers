using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandbookSelector : MonoBehaviour
{
    [SerializeField] int indexChange;

    public void OnClick()
    {
        HandbookManager.Instance.currentIndex += indexChange;
        HandbookManager.Instance.ChangeEnemy();
    }
}
