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
        //�X�^�[�g���j���[��gamestart_button_click��ture�ɂȂ�����V�[���J��
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Test_Select_Scene");
    }
    public void GameExit()
    {
        //�Q�[���̏I��
        //�G�f�B�^�[��ł͌Ă΂�Ȃ��̂ŁA�G�f�B�^�[�ł̓f�o�b�O���I��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //�A�v���P�[�V������ł̏I��
#else
        Application.Quit();
#endif
    }
}
