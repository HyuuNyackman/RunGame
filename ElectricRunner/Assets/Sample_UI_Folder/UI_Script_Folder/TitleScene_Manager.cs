using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScene_Manager : MonoBehaviour
{
    //public GameObject FadePanel;
    private FadeManager fadeManager; 

    void Start()
    {
        //Invoke("FindFadeObject", 0.02f);
        FindFadeObject();
    }
    void FindFadeObject()
    {
        fadeManager = GameObject.Find("FadeCanvas").GetComponent<FadeManager>();
        //fadeManager = FadePanel.GetComponent<FadeManager>();
        fadeManager.FadeIn();
    }
    public async void GameStrat()
    {
        //スタートメニューでgamestart_button_clickがtureになったらシーン遷移
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Test_Select_Scene");
    }
    public void GameExit()
    {
        //ゲームの終了
        //エディター上では呼ばれないので、エディターではデバッグを終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //アプリケーション上での終了
#else
        Application.Quit();
#endif
    }
}
