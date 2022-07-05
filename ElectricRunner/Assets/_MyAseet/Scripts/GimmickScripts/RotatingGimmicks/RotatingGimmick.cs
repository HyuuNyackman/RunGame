using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGimmick : GimmickManager
{
    private float moveTime;
    private Vector3 firstPosition;
    [SerializeField] float plus;
    [SerializeField] float speed;
    [SerializeField] GameObject rotateOb;

    // Start is called before the first frame update
   protected override void Start()
    {
        base.Start();
        firstPosition=rotateOb.transform.position;
    }

    // Update is called once per frame
   protected override void Update()
    {
       base.Update();
    }
    protected override void Current()
    {
            if (rotateOb.transform.position.y < firstPosition.y + plus)
            {
                rotateOb.transform.position += Vector3.up * (speed * Time.deltaTime);
            }
    }
    protected override void CurrentOff()
    {
        if (rotateOb.transform.position.y > firstPosition.y)
        {
            rotateOb.transform.position += Vector3.down * (speed * Time.deltaTime);
        }
    }
}
