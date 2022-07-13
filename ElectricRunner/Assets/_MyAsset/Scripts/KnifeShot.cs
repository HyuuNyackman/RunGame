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
    [SerializeField, Tooltip("ナイフを戻すのにかかる時間")] float returnDuration;
    [SerializeField, Tooltip("ナイフを飛ばせる距離")] float maxWireLength;

    [SerializeField] private Transform knife;
    [SerializeField] Transform mainCam;
    [SerializeField] private float assistRayRadius;
    [SerializeField] float assistRange = 10;
    [SerializeField] GameObject knifeTrail;


    //  ナイフを戻す場所(スタート時のローカル座標)
    Vector3 knifeBackPoint;

    //その他
    RaycastHit hit;

    //  ナイフの親オブジェクト
    Transform parentObj;

    private bool knifeReturns;              //  ナイフを戻している最中か
    private DestroyKnife destroyKnife;

    public Vector3 GetHookFlyingPoint => destroyKnife.GetHookFlyingPoint;
    public bool KnifeHolds { get; private set; }        //  ナイフを持っているか
    public bool CanHookFlying { get; private set; }     //  ナイフの場所に飛べるか

    private void Start()
    {
        destroyKnife = knife.GetComponent<DestroyKnife>();

        //  現在のナイフのローカル座標を覚えておく
        knifeBackPoint = knife.transform.localPosition;
        //  現在の親オブジェクトを入れる
        parentObj = knife.transform.parent;

        //  初期化する
        OnHoldKnife();
    }
    public void Update()
    {
        //一定距離離れたらナイフを戻す
        float dis = Vector3.Distance(mainCam.position, knife.transform.position);

        if (dis > maxWireLength)
        {
            ReturnKnife();
        }

        CanHookFlying = destroyKnife.Sticks;
    }
    private void OnTriggerEnter(Collider other)
    {
        //  ナイフが刺さっているか、戻している時に
        //  ナイフに触れたら手元に戻す
        if (other.gameObject.CompareTag("Knife") && (destroyKnife.Sticks || knifeReturns))
        {
            //  ナイフをつかむ
            OnHoldKnife();
        }
    }

    /// <summary>
    /// ナイフを投げる
    /// </summary>
    public void ThrowingKnife()
    {
        if (!KnifeHolds)
        {
            return;
        }

        Vector3 throwPoint;
        Vector3 throwDirection;

        if (IsHitTarget())
        {
            //  ヒットしたオブジェクトの原点に飛ばす
            throwPoint = hit.collider.transform.position;
            //  ヒットした場所に飛ばすならこっち
            //shootPoint = hit.point;

            throwDirection = (throwPoint - knife.position).normalized;
            knife.transform.LookAt(throwPoint);
        }
        else
        {
            throwDirection = mainCam.forward;
            knife.transform.forward = throwDirection;
        }

        //エフェクト
        knifeTrail.SetActive(true);

        KnifeHolds = false;
        knife.transform.parent = null;
        destroyKnife.Throw(speed, throwDirection);
    }

    /// <summary>
    /// ナイフを手元に戻す
    /// </summary>
    public void ReturnKnife()
    {
        if (KnifeHolds || knifeReturns)
        {
            return;
        }

        knifeReturns = true;
        Vector3 moveDirection = (mainCam.position - knife.position).normalized;
        destroyKnife.Return(returnDuration, moveDirection);
    }

    /// <summary>
    /// ナイフが完全に戻ってきたとき
    /// </summary>
    private void OnHoldKnife()
    {
        KnifeHolds = true;
        knifeReturns = false;

        //  ナイフの動きを止める
        destroyKnife.Hold();

        //  エフェクトを止める
        knifeTrail.SetActive(false);

        //  元の場所に戻す
        knife.transform.parent = parentObj;
        knife.transform.localPosition = knifeBackPoint;
        knife.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    /// <summary>
    /// ターゲットにレイが当たったか調べる
    /// </summary>
    /// <returns>レイが当たったか</returns>
    private bool IsHitTarget()
    {
        bool isHit = false;

        if (Physics.SphereCast(mainCam.position, assistRayRadius, mainCam.forward * assistRange, out hit))
        {
            //Rayの当たったオブジェクトがtargetタグだった場合
            if (hit.collider.CompareTag("Target"))
            {
                //Debug.Log("Targetが有効");
                isHit = true;
            }
        }

        return isHit;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(mainCam.position, aimAssistRayRadius);
    //     if (isHit)
    //     {
    //         Gizmos.color = Color.red;
    //     }
    // }
}

