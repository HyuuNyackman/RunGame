using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_Manager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private FadeManager fadeManager;
    public bool IsPause { get; private set; }
    void Start()
    {
        FindFadeObject();
    }

    private void Update()
    {

    }

    public async void OnPushRestartButton()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //ゲームシーンからSelectへ行くボタン関数
    public async void OnPushBackSelectButton()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Select_Scene");
    }

    //ゲームシーンからTitleへ行くボタン関数
    public async void OnPushBackTitleButton()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Title_Scene");
    }

    //ゲーム自体から脱出する関数
    public void OnPushExitGameButton()
    {
        //エディター上では呼ばれないので、エディターではデバッグを終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //アプリケーション上での終了
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// ポーズトリガーを押したときの処理
    /// </summary>
    public void OnPushPauseTrigger()
    {
        if (IsPause)
        {
            pauseCanvas.SetActive(false);
            //ポーズ機能
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            IsPause = false;
        }
        else
        {
            pauseCanvas.SetActive(true);
            //ポーズ機能
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            IsPause = true;
        }
    }

    private void FindFadeObject()
    {
        //フェードキャンバスをさがす
        fadeManager = GameObject.Find("FadeCanvas").GetComponent<FadeManager>();
        fadeManager.FadeIn();
        Time.timeScale = 1;
    }
}
