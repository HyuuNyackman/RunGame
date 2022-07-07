using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //デバッグモード有効化
    [SerializeField] private bool debugFallDeath = false;
    [SerializeField] private bool debugSuicide=false;

    //以下アイテムの格納
    [SerializeField] private GameObject returnToGameCanvas;
    [SerializeField] private GameObject player;

    //以下落下死条件
    [Header("正の数で入力してオッケーなはず")]
    [SerializeField] private float gameOverLine = 50.0f;


    //リスポーン関係
    [SerializeField] GameObject parentRespawnObj;
    [SerializeField] List< RespawnPoint> respawnPoints =new List<RespawnPoint>();
    [SerializeField] bool respornBool;
    //[SerializeField] GameObject[] respoObj;
    
   

    private void Start()
    {
        // プレイヤーのリスポーン
        //Debug.Log(RespawnPoint.RespawnNumber);
        respornBool = false;
        player.transform.position = respawnPoints[RespawnPoint.RespawnNumber].Respawn(player.transform.position);
    }
   

    public void GameOver()
    {
        returnToGameCanvas.SetActive(true);
        //再生を停止
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    //リトライボタンの関数
    public void ReLoadToGameScene()
    {
        respornBool = true;
        Time.timeScale = 1;


        returnToGameCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        respornBool = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
    void Update()
    {
        respawnPoints[RespawnPoint.RespawnNumber].CurrentBool = false;
        //ふつうの死亡
        if(debugSuicide==true/*&& ここに本来の死亡判定*/)
        {
            GameOver();
        }
        //落下死
        if (debugFallDeath/*&& ここに本来の死亡判定*/)
        {
            //ここのコメントアウトをけして使う
            //GameOver();

            //プレイヤーがワールド座標のgameOverLineの高さまで落ちたら死
            if (player.transform.position.y <= gameOverLine * -1 && respornBool == false)
            {
                GameOver();
            }
        }
    }
    public int RespornNumber(RespawnPoint respawnPoint)
    {
        return respawnPoints.IndexOf(respawnPoint);
    }

}
