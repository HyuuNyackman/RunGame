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
        //�t�F�[�h�L�����o�X��������
        fadeManager = GameObject.Find("FadeCanvas").GetComponent<FadeManager>();
        fadeManager.FadeIn();
        Time.timeScale = 1;
    }
    //�G�X�P�[�v�{�^���̓���
    public void Push_Esc()
    {
        escuicanvas.SetActive(true);
        //�|�[�Y�@�\
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
   
    //--�f�o�b�O�p�A�b�v�f�[�g�A��Ɋ��p���邩��-------\\
    private void Update()
    {
        //kyeboard�擾
        var Current = Keyboard.current;
        //kyeboard�@null�@�`�F�b�N
        if (Current == null)
        {
            return;
        }
        //�G�X�P�[�v�̓��͊m�F
        var Esc = Current.escapeKey;
        //�G�X�P�[�v�܂��f�o�b�O���L���ł���Γ���m�F
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
    //�Q�[���V�[������Selet�֍s���{�^���֐�
    public async void Return_Select()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Select_Scene");
    }
    //�Q�[���V�[������Title�֍s���{�^���֐�
    public async void Rrturn_Title()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Title_Scene");
    }
    //�Q�[�����̂���E�o����֐�
    public  void Game_Exit()
    {
        //�G�f�B�^�[��ł͌Ă΂�Ȃ��̂ŁA�G�f�B�^�[�ł̓f�o�b�O���I��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //�A�v���P�[�V������ł̏I��
#else
        Application.Quit();
#endif
    }
}
