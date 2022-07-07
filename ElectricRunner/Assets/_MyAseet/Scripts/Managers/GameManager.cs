using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�f�o�b�O���[�h�L����
    [SerializeField] private bool debugFallDeath = false;
    [SerializeField] private bool debugSuicide=false;

    //�ȉ��A�C�e���̊i�[
    [SerializeField] private GameObject returnToGameCanvas;
    [SerializeField] private GameObject player;

    //�ȉ�����������
    [Header("���̐��œ��͂��ăI�b�P�[�Ȃ͂�")]
    [SerializeField] private float gameOverLine = 50.0f;


    //���X�|�[���֌W
    [SerializeField] GameObject parentRespawnObj;
    [SerializeField] List< RespawnPoint> respawnPoints =new List<RespawnPoint>();
    [SerializeField] bool respornBool;
    //[SerializeField] GameObject[] respoObj;
    
   

    private void Start()
    {
        // �v���C���[�̃��X�|�[��
        //Debug.Log(RespawnPoint.RespawnNumber);
        respornBool = false;
        player.transform.position = respawnPoints[RespawnPoint.RespawnNumber].Respawn(player.transform.position);
    }
   

    public void GameOver()
    {
        returnToGameCanvas.SetActive(true);
        //�Đ����~
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    //���g���C�{�^���̊֐�
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
        //�ӂ��̎��S
        if(debugSuicide==true/*&& �����ɖ{���̎��S����*/)
        {
            GameOver();
        }
        //������
        if (debugFallDeath/*&& �����ɖ{���̎��S����*/)
        {
            //�����̃R�����g�A�E�g�������Ďg��
            //GameOver();

            //�v���C���[�����[���h���W��gameOverLine�̍����܂ŗ������玀
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
