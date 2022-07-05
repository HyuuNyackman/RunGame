using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChange_Manager : MonoBehaviour
{
  [SerializeField] private bool debugEnabled = false;
  [SerializeField] private GameObject escuicanvas;
  //ゲームオーバー(落下死)試験的実装
  [SerializeField] private GameObject gameOverCanvas;
  [SerializeField] private GameObject player;
  [Header("正の数で入力してオッケーなはず")]
  [SerializeField] private float gameOverLine = 50.0f;
  //試験ここまで
  private FadeManager fadeManager;
  void Start()
  {
    //Invoke("FindFadeObject", 0.02f);
    FindFadeObject();
  }
  void FindFadeObject()
  {
    fadeManager = GameObject.Find("FadeCanvas").GetComponent<FadeManager>();
    fadeManager.FadeIn();
    Time.timeScale = 1;
  }
  public void Push_Esc()
  {
    escuicanvas.SetActive(true);
    //ポーズ機能
    Time.timeScale = 0;
    Cursor.lockState = CursorLockMode.None;
  }
  //--ここ試験的実装---------------------------------\\
  public void GameOver()
  {
    escuicanvas.SetActive(true);
    gameOverCanvas.SetActive(true);
    //ポーズ機能
    Time.timeScale = 0;
    Cursor.lockState = CursorLockMode.None;
  }
  //--ここまで------------------------------------------\\
  //--デバッグ用アップデート、後に活用するかも-------\\
  private void Update()
  {
    //kyeboard取得
    var Current = Keyboard.current;
    //kyeboard　null　チェック
    if (Current == null)
    {
      return;
    }
    //エスケープの入力確認
    var Esc = Current.escapeKey;
    //エスケープまたデバッグが有効であれば動作確認
    if (Esc.wasPressedThisFrame && debugEnabled == true)
    {
      Push_Esc();
    }
    //--ここ試験的実装----------------------------------------------------------\\
    //  Debug.Log("アップデートは回っている");
    //プレイヤーがワールド座標のgameOverLineの高さまで落ちたら死
    if (player.transform.position.y <= gameOverLine * -1)
    {
      Debug.Log("しんだよ");
      GameOver();
    }
    //--ここまで-------------------------------------------------------------------\\
    //--デバッグ用エスケープの入力もここまで------------------------------------\\
  }

  //ゲームシーンからSeletへ行くボタン関数
  public async void Return_Select()
  {
    fadeManager.FadeOut();
    await Task.Delay(200);
    SceneManager.LoadScene("Test_Select_Scene");
  }
  //ゲームシーンからTitleへ行くボタン関数
  public async void Rrturn_Title()
  {
    fadeManager.FadeOut();
    await Task.Delay(200);
    SceneManager.LoadScene("Test_Title_Scene");
  }
  //ゲーム自体から脱出する関数
  public void Game_Exit()
  {
    //エディター上では呼ばれないので、エディターではデバッグを終了
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    //アプリケーション上での終了
#else
        Application.Quit();
#endif
  }
}
