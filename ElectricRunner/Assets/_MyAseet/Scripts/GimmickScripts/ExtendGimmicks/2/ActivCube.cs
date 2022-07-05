using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivCube : MonoBehaviour
{
    private ExtendCube2 ec2;

    private void OnDisable()
    {
       // Ec2.i_int--;
    }
    public ExtendCube2 Extend2
    {
        get { return ec2; }
        set { ec2 = value; }
    }
}
