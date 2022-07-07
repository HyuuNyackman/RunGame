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
  
    private FadeManager fadeManager;
    void Start()
    {
        //Invoke("FindFadeObject", 0.02f);
        FindFadeObject();
    }
    void FindFadeObject()
    {
        //フェードキャンバスをさがす
        fadeManager = GameObject.Find("FadeCanvas").GetComponent<FadeManager>();
        fadeManager.FadeIn();
        Time.timeScale = 1;
    }
    //エスケープボタンの入力
    public void Push_Esc()
    {
        escuicanvas.SetActive(true);
        //ポーズ機能
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
   
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
    }
    public async void ReStart()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //ゲームシーンからSeletへ行くボタン関数
    public async void Return_Select()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Select_Scene");
    }
    //ゲームシーンからTitleへ行くボタン関数
    public async void Rrturn_Title()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Title_Scene");
    }
    //ゲーム自体から脱出する関数
    public  void Game_Exit()
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
