//////////////////////////////////////////////////////////////////////////////////
///　用途
/// プレイヤーにソースコードを追加する
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
    // float変数
    [SerializeField] float speed;
    public float Radius;

    //bool変数
    public bool Return;
    public bool Shot = false;

    // GameObject変数
    public GameObject Knife;
    public GameObject ShotPoint;

    [SerializeField] Transform mainCam;
    [SerializeField] float assistRange=10;
    [SerializeField] bool isHit;
    public bool HoldHand = true;
    [SerializeField] GameObject knifeTraile;
 
    //ナイフを打ち出す場所
    //public GameObject KnifeShotPoint;
    //  ナイフを戻す場所(スタート時のローカル座標)
    Vector3 knifeBackPoint;

    //RigidBody変数
    Rigidbody rb;

    //その他
    RaycastHit hit;
    // MeshRenderer mesh;

    //  ナイフの親オブジェクト
    Transform parentObj;

    //プレイヤー移動用
    //private CharacterController characterController;
    private void Start()
    {
        // mesh = Knife.GetComponent<MeshRenderer>();
        // mesh.enabled = false;
        //  characterController = GetComponent<CharacterController>();
        // Vector3 knife = Knife.transform.position;
        rb = Knife.GetComponent<Rigidbody>();

        //  現在のナイフのローカル座標を覚えておく
        knifeBackPoint = Knife.transform.localPosition;
        //  現在の親オブジェクトを入れる
        parentObj = Knife.transform.parent;
    }
    public void Update()
    {
       
        // //マウス右を押したときに回収
        // {
        //     ReCharge();
        // }
        // //左マウスボタンを押した際にショット
        // //Rayに何も引っ掛かってない時の処理
        // {
        //     ThrowingKnife();
        // }
        // if (Knife.GetComponent<DestroyKnife>().Stop)
        // {
        //     HookFlyingMovement();
        // }
    }
    public void ThrowingKnife()
    {
        isHit = Physics.SphereCast(mainCam.position, Radius, mainCam.forward * assistRange, out hit);

        if (isHit)
        {
            //Rayの当たったオブジェクトがtargetタグだった場合
            if (hit.collider.tag == "Target")
            {
                Debug.Log("Targetが有効");

                //そのオブジェクトをShotPoint変数に格納
                ShotPoint = hit.collider.gameObject;
                //if (Physics.CheckSphere(transform.position, Radius))
                //{

                //    ShotPoint = hit.collider.gameObject;
                //}
            }
            else
            {
                ShotPoint = null;
            }

        }
        else
        {
            ShotPoint = null;
        }
        


        if (Shot == false && ShotPoint == null)
        {
            Return = false;
            // mesh.enabled = true;
            Shot = true;

            //エフェクト
            knifeTraile.SetActive(true);

            //自分の場所に戻す
            Knife.transform.parent = parentObj;
            Knife.transform.localPosition = knifeBackPoint;

            //ナイフのショット
            Knife.transform.parent = null;
            rb.isKinematic = false;
            Vector3 force = mainCam.forward * speed;
            rb.AddForce(force, ForceMode.Impulse);
        }
        //Rayに引っ掛かってるときの処理
        else if (Shot == false && ShotPoint != null)
        {
            Return = false;
            // mesh.enabled = true;
            Shot = true;

            //自分の場所に戻す
            Knife.transform.parent = parentObj;
            Knife.transform.localPosition = knifeBackPoint;

            //発射台とターゲットのベクトルを計算する
            Vector3 knifeShotPoint = Knife.transform.position;
            Vector3 GameObjectPos = knifeShotPoint;
            Vector3 TargetPos = ShotPoint.transform.position;
            //Vector3 shotForward = Vector3.Scale((TargetPos - GameObjectPos), new Vector3(1, 1, 1)).normalized;
            Vector3 shotForward= (TargetPos - GameObjectPos).normalized;

            //ナイフのショット
            Knife.transform.parent = null;
            rb.isKinematic = false;
            //Vector3 force = new Vector3(shotForward.x, shotForward.y, shotForward.z);
            //rb.AddForce(force * speed, ForceMode.Impulse);
            rb.AddForce(shotForward*speed, ForceMode.Impulse);
            knifeTraile.SetActive(true);
        }
        knifeTraile.SetActive(false);
    }
    //ナイフの刺さった位置へ飛ぶ
    public void HookFlyingMovement()
    {
        if (Knife.GetComponent<DestroyKnife>().Stop)
        {
            Vector3 hookPoint = Knife.transform.position;
            Vector3 moveDir = (hookPoint - transform.position).normalized;
            float flyingSpeed = Vector3.Distance(transform.position, hookPoint) * 2f;
            //  characterController.Move(moveDir * flyingSpeed * Time.deltaTime);
        }
    }
    public void ReCharge()
    {
        //回収時に元の位置に戻す
        Return = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        // mesh.enabled = false;
        Shot = false;

        //自分の場所に戻す
        Knife.transform.parent = parentObj;
        Knife.transform.localPosition = knifeBackPoint;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(mainCam.position, Radius);
        if (isHit) 
        {
            Gizmos.color= Color.red;
        }
    }
}

