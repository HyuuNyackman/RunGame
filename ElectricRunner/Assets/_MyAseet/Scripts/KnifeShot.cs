//////////////////////////////////////////////////////////////////////////////////
///　用途
/// 空のゲームオブジェクトにソースコードを追加する
///
/// 効果
/// オブジェクトの向いている方向にプレハブオブジェクトを打ち出す
/// 
//////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//視野の範囲内にいるときは方向ベクトルを取得、範囲外なら普通に飛ばす
public class KnifeShot : MonoBehaviour
{
    [SerializeField] Transform aimTarget;
    // float変数
    public float Speed;
    public float Radius;
    //視野角
    public float SightAngle = 5.0f;
    private float time;
    [SerializeField]private float limit = 3;

    //bool変数
    public bool Return;
    private bool shot = false;

    // GameObject変数
    public GameObject Knife;
    public GameObject ShotPoint;

    //RigidBody変数
    Rigidbody rb;

    //その他
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
            //Rayの当たったオブジェクトがtargetタグだった場合
            if (hit.collider.tag == "Target")
            {
                //そのオブジェクトをShotPoint変数に格納
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
        //左マウスボタンを押した際にショット
        //Rayに何も引っ掛かってない時の処理
        if (shot == false && ShotPoint == null)
        {
            Return = false;
            shot = true;
            rb = Instantiate(Knife,aimTarget.position,transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(aimTarget.forward * Speed, ForceMode.Impulse);
        }
        //Rayに引っ掛かってるときの処理
        else if (shot == false && ShotPoint != null)
        {
            Return = false;
            shot = true;

            //発射台とターゲットのベクトルを計算する
            Vector3 GameObjectPos = aimTarget.position;
            Vector3 TargetPos = ShotPoint.transform.position;
            Vector3 shotForward = Vector3.Scale((TargetPos - aimTarget.position), new Vector3(1, 1, 1)).normalized;

            //ナイフの生成からショット
            rb = Instantiate(Knife, aimTarget.position, transform.rotation).GetComponent<Rigidbody>();
            Vector3 force = new Vector3(shotForward.x, shotForward.y, shotForward.z);
            rb.AddForce(force * Speed, ForceMode.Impulse);
        }
        //右マウスボタンを押した際にリチャージ
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

