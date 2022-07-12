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
    [SerializeField] float Distance;
    public float Radius;

    //bool変数
    public bool Return;
    public bool Shot = false;
    public bool Chatch;
    // private bool back;

    // GameObject変数
    public GameObject Knife;
    public GameObject ShotPoint;

    [SerializeField] Transform mainCam;
    [SerializeField] float assistRange = 10;
    [SerializeField] bool isHit;
    [SerializeField] GameObject knifeTrail;


    //ナイフを打ち出す場所
    // public GameObject KnifeShotPoint;
    //  ナイフを戻す場所(スタート時のローカル座標)
    Vector3 knifeBackPoint;

    //RigidBody変数
    Rigidbody rb;

    //その他
    RaycastHit hit;

    //  ナイフの親オブジェクト
    Transform parentObj;

    private DestroyKnife destroyKnife;

    public Vector3 GetKnifePosition => Knife.transform.position;
    public bool CanHookFlying { get; private set; }
    private void Start()
    {
        rb = Knife.GetComponent<Rigidbody>();
        destroyKnife = Knife.GetComponent<DestroyKnife>();

        //  現在のナイフのローカル座標を覚えておく
        knifeBackPoint = Knife.transform.localPosition;
        //  現在の親オブジェクトを入れる
        parentObj = Knife.transform.parent;
    }
    public void Update()
    {
        //一定距離離れたらリチャージ
        float dis = Vector3.Distance(mainCam.position, Knife.transform.position);

        // //マウス右を押したときに回収
        // if (Input.GetMouseButtonDown(1) && Shot)
        // {
        //     back = true;
        // }
        if (dis > Distance)
        {
            ReCharge();
        }

        CanHookFlying = destroyKnife.Stop;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife")
        {
            Return = false;
            Shot = false;
            //自分の場所に戻す
            Knife.transform.parent = parentObj;
            Knife.transform.localPosition = knifeBackPoint;
            Knife.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
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

        if (Shot == false && ShotPoint == null && Return == false)
        {
            //エフェクト
            knifeTrail.SetActive(true);

            //ナイフのショット
            Knife.transform.parent = null;
            Shot = true;
            rb.isKinematic = false;
            Knife.transform.rotation = Quaternion.LookRotation(mainCam.forward);
            Vector3 force = mainCam.forward * speed;
            rb.AddForce(force, ForceMode.Impulse);
        }
        //Rayに引っ掛かってるときの処理
        else if (Shot == false && Return == false)
        {
            //エフェクト
            knifeTrail.SetActive(true);

            Knife.transform.parent = null;
            Shot = true;
            rb.isKinematic = false;

            //発射台とターゲットのベクトルを計算する
            Vector3 knifeShotPoint = Knife.transform.position;
            Vector3 GameObjectPos = knifeShotPoint;
            Vector3 TargetPos = ShotPoint.transform.position;
            Vector3 shotForward = (TargetPos - GameObjectPos).normalized;
            Knife.transform.rotation = Quaternion.LookRotation(ShotPoint.transform.position);

            //ナイフのショット
            rb.AddForce(shotForward * speed, ForceMode.Impulse);
        }
        else if (ShotPoint == null)
        {

        }
    }
    public void ReCharge()
    {
        //回収時に元の位置に戻す
        Return = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        Shot = false;
        //自分の場所に戻す
        if (Chatch)
        {
            Knife.transform.parent = parentObj;
            Knife.transform.localPosition = knifeBackPoint;
            Chatch = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(mainCam.position, Radius);
        if (isHit)
        {
            Gizmos.color = Color.red;
        }
    }
}

