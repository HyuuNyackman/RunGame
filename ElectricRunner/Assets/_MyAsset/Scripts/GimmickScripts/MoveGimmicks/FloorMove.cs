using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HoldはTargetを1つで刺さってる時だけ着く
//Chargedは複数Targetを置け刺さってない時でも一定時間帯電状態
public class FloorMove : GimmickManager
{
    public enum GimickMoveType
    {
        XMove,
        YMove,
        ZMove,
    };
    [SerializeField] GimickMoveType moveType;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxMoveTime;

    private float moveTime;


    private int B;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveTime = 0;


    }

    // Update is called once per frame
    protected override void Update()
    {
        //Current();
        base.Update();
    }



    // ギミックの動き
    protected override void Current()
    {
        if (moveTime < maxMoveTime)
        {
            moveTime += Time.deltaTime;
            if (moveType == GimickMoveType.XMove)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
            if (moveType == GimickMoveType.YMove)
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
            if (moveType == GimickMoveType.ZMove)
            {
                transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

}
