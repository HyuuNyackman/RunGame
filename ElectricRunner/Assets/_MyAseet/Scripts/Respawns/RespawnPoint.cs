using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : RangeGimmickManager
{
    public static int RespawnNumber;
    [SerializeField] GameObject respawnPointObj;
    [SerializeField] GameManager gameManager;
    //public bool CurrentBool { get; set; }

    protected override void Update()
    {
            base.Update();
    }
    /// <summary>
    /// RespawnNumber�̏�����
    /// ���g���C�ȊO�̎��Ăяo��
    /// </summary>
    public void InitRespawnNumber()
    {
        RespawnNumber = 0;
    }

    protected override void StartUp()
    {
        RespawnNumber = gameManager.RespornNumber(this);
        Debug.Log(RespawnNumber);

    }

    /// <summary>
    /// �v���C���[�̃|�W�V������Ԃ�
    /// </summary>
    /// <param name="position">�v���C���[�̃|�W�V����</param>
    /// <returns>�|�W�V������Ԃ�</returns>
    public Vector3 Respawn(Vector3 position)
    {
        position = respawnPointObj.transform.position;
        return position;
    }
}
