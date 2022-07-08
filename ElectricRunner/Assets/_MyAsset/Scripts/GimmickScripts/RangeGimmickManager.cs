using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeGimmickManager : MonoBehaviour
{
    public bool CurrentBool { get; set; }

    protected abstract void StartUp();

    // Start is called before the first frame update
   protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CurrentBool)
        {
            StartUp();
        }
    }
}
