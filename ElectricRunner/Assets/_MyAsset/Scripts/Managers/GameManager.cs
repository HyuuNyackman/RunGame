using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //以下アイテムの格納
    [SerializeField] private GameObject returnToGameCanvas;
    [SerializeField] private GameObject player;

    //リスポーン関係
    [SerializeField] GameObject parentRespawnObj;
    [SerializeField] List<RespawnPoint> respawnPoints = new List<RespawnPoint>();
    //[SerializeField] GameObject[] respoObj;

    //ゲームオーバー関係
    public static bool IsPlayerDeath { get; private set; }

    private void Start()
    {
        IsPlayerDeath = false;
        // プレイヤーのリスポーン
        //Debug.Log(RespawnPoint.RespawnNumber);
        player.transform.position = respawnPoints[RespawnPoint.RespawnNumber].Respawn(player.transform.position);
    }


    public void GameOver()
    {
        IsPlayerDeath = true;
        returnToGameCanvas.SetActive(true);
        //再生を停止
        Time.timeScale = 0;

        //  カーソルを出す
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    //リトライボタンの関数
    public void ReLoadToGameScene()
    {
        Time.timeScale = 1;

        returnToGameCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        respawnPoints[RespawnPoint.RespawnNumber].CurrentBool = false;
    }
    public int RespornNumber(RespawnPoint respawnPoint)
    {
        return respawnPoints.IndexOf(respawnPoint);
    }

}
