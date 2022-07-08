using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionArea : MonoBehaviour
{
    [SerializeField] RangeGimmickManager rangeGimmickManager;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rangeGimmickManager.CurrentBool = true;
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rangeGimmickManager.CurrentBool = true;
        }
    }
}
