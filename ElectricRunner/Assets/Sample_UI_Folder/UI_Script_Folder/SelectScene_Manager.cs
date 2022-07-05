using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;



public class SelectScene_Manager : MonoBehaviour
{
    private FadeManager fadeManager;
    //Button�� On Click ���� SelectScene_Maneger.Select_Stage_Button
    //�̊֐����w��B�֐��Ɉ���(int�^)������̂ŁA�쐬�����X�e�[�W
    //��uStage4�v�ł���΁g�S�h���w�肷�邱�ƂŃX�e�[�W�Ăяo�����\

    [SerializeReference] private TextMeshProUGUI text_number;
    //�ʏ�X�e�[�W�Z���N�g�̃{�^���֐�
    //Starge+int �l�Ń��[�h����V�[���̑I�����\
    void Start()
    {
        //Invoke("FindFadeObject", 0.02);
        FindFadeObject();
    }
    void FindFadeObject()
    {
        fadeManager = GameObject.Find("FadeCanvas").GetComponent<FadeManager>();
        fadeManager.FadeIn();
    }
    public async void Select_Stage_Button(int number)
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        text_number.text = "" + number;
        SceneManager.LoadScene("Stage" + number.ToString());
    }
    //�`���[�g���A���p�̃{�^���֐�
    //�`���[�g���A���ȊO�ɐݒ肵�Ȃ��I
    public async void Select_Stage_Tutoreal()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Tutorial_Scene");
    }
}
