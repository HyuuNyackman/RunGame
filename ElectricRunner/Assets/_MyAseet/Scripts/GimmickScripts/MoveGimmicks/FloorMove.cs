using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hold��Target��1�Ŏh�����Ă鎞��������
//Charged�͕���Target��u���h�����ĂȂ����ł���莞�ԑѓd���
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



    // �M�~�b�N�̓���
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
