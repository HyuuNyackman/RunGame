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
    //Buttonの On Click から SelectScene_Maneger.Select_Stage_Button
    //の関数を指定。関数に引数(int型)があるので、作成したステージ
    //例「Stage4」であれば“４”を指定することでステージ呼び出しが可能

    [SerializeReference] private TextMeshProUGUI text_number;
    //通常ステージセレクトのボタン関数
    //Starge+int 値でロードするシーンの選択が可能
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
    //チュートリアル用のボタン関数
    //チュートリアル以外に設定しない！
    public async void Select_Stage_Tutoreal()
    {
        fadeManager.FadeOut();
        await Task.Delay(200);
        SceneManager.LoadScene("Tutorial_Scene");
    }
}
