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
    /// RespawnNumberの初期化
    /// リトライ以外の時呼び出す
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
    /// プレイヤーのポジションを返す
    /// </summary>
    /// <param name="position">プレイヤーのポジション</param>
    /// <returns>ポジションを返す</returns>
    public Vector3 Respawn(Vector3 position)
    {
        position = respawnPointObj.transform.position;
        return position;
    }
}
