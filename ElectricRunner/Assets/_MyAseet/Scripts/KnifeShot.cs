//////////////////////////////////////////////////////////////////////////////////
///�@�p�r
/// ��̃Q�[���I�u�W�F�N�g�Ƀ\�[�X�R�[�h��ǉ�����
///
/// ����
/// �I�u�W�F�N�g�̌����Ă�������Ƀv���n�u�I�u�W�F�N�g��ł��o��
/// 
//////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����͈͓̔��ɂ���Ƃ��͕����x�N�g�����擾�A�͈͊O�Ȃ畁�ʂɔ�΂�
public class KnifeShot : MonoBehaviour
{
    [SerializeField] Transform aimTarget;
    // float�ϐ�
    public float Speed;
    public float Radius;
    //����p
    public float SightAngle = 5.0f;
    private float time;
    [SerializeField]private float limit = 3;

    //bool�ϐ�
    public bool Return;
    private bool shot = false;

    // GameObject�ϐ�
    public GameObject Knife;
    public GameObject ShotPoint;

    //RigidBody�ϐ�
    Rigidbody rb;

    //���̑�
    RaycastHit hit;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        var isHit = Physics.SphereCast(aimTarget.position, Radius, transform.forward * 10, out hit);
        if (isHit)
        {
            //Ray�̓��������I�u�W�F�N�g��target�^�O�������ꍇ
            if (hit.collider.tag == "Target")
            {
                //���̃I�u�W�F�N�g��ShotPoint�ϐ��Ɋi�[
                ShotPoint = hit.collider.gameObject;
            }
        }
        else
        {
            ShotPoint = null;
        }
        //Shot();
    }
    public void Shot()
    {
        //���}�E�X�{�^�����������ۂɃV���b�g
        //Ray�ɉ��������|�����ĂȂ����̏���
        if (shot == false && ShotPoint == null)
        {
            Return = false;
            shot = true;
            rb = Instantiate(Knife,aimTarget.position,transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(aimTarget.forward * Speed, ForceMode.Impulse);
        }
        //Ray�Ɉ����|�����Ă�Ƃ��̏���
        else if (shot == false && ShotPoint != null)
        {
            Return = false;
            shot = true;

            //���ˑ�ƃ^�[�Q�b�g�̃x�N�g�����v�Z����
            Vector3 GameObjectPos = aimTarget.position;
            Vector3 TargetPos = ShotPoint.transform.position;
            Vector3 shotForward = Vector3.Scale((TargetPos - aimTarget.position), new Vector3(1, 1, 1)).normalized;

            //�i�C�t�̐�������V���b�g
            rb = Instantiate(Knife, aimTarget.position, transform.rotation).GetComponent<Rigidbody>();
            Vector3 force = new Vector3(shotForward.x, shotForward.y, shotForward.z);
            rb.AddForce(force * Speed, ForceMode.Impulse);
        }
        //�E�}�E�X�{�^�����������ۂɃ��`���[�W
        Recharge();
    }
    public void Recharge()
    {
        if (shot == true)
        {
            {
                shot = false;
                time = 0;
                Return = true;
            }
        }
    }
}

