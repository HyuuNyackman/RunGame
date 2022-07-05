using
    System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendCube : GimmickManager
{
    [SerializeField] GameObject extendOb;
    [SerializeField] float extendSpeed;
    [SerializeField] float maxLength;

   protected override void Update()
    {
        base.Update();
    }
    protected override void Current()
    {
        if (extendOb.transform.localScale.x <= maxLength)
        {
            float ExS = extendSpeed + Time.deltaTime;
            extendOb.transform.localScale = new Vector3(extendOb.transform.localScale.x + ExS, extendOb.transform.localScale.y, extendOb.transform.localScale.z);
        }
    }
}
