using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GimmickManager : MonoBehaviour
{
   TargetHit[] targethit;
    protected int Count;
   [SerializeField] GameObject targetObj;
    private bool allCount;
    public int CurrentCount
    {
        get { return Count; }
        set { Count = value; }
    }
    public bool AllCount
    {
        get { return allCount; }
    }

    protected virtual void Start()
    {
        int childCount = targetObj.transform.childCount;
        targethit = new TargetHit[childCount];

        for (int i = 0; i < targethit.Length; i++)
        {
            targethit[i] = targetObj.transform.GetChild(i).gameObject.GetComponent<TargetHit>();
        }
    }

    protected abstract void Current();

    protected virtual void CurrentOff()
    {

    }


    protected virtual void Update()
    {
        if (allCount)
        {
            Current();
        }
        else
        {
            CurrentOff();
        }

        if (Count >= targethit.Length)
        {
            allCount = true;
        }
        if (Count < targethit.Length)
        {
            allCount = false;
        }
    }

    
}
