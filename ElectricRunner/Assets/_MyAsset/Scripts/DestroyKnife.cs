using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyKnife : MonoBehaviour
{
    [SerializeField] private Transform sticksPoint;
    [SerializeField] private Transform hookFlyingPoint;

    [SerializeField] ParticleSystem hitLightning;
    [SerializeField] ParticleSystem hitSpark;

    //RigidBody変数
    Rigidbody rb;

    private Transform mainCamera;
    private Vector3 returnBeginPosition;
    private float returnDuration;
    private float returnRate;

    private bool throws;    //  投げられているか
    private bool returns;   //  戻しているか

    public Vector3 GetHookFlyingPoint => hookFlyingPoint.position;
    public bool Sticks { get; private set; }    //  刺さっているか
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (returns)
        {
            returnRate += 1 / returnDuration * Time.deltaTime;
            rb.position = Vector3.Lerp(returnBeginPosition, mainCamera.position, returnRate);

            if (returnRate >= 1)
            {
                returns = false;
            }
        }

    }

    //刺さったときに止まる
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target") || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
        {
            //  投げている時だったら止める
            if (throws)
            {
                throws = false;
                returns = false;
                Sticks = true;
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;

                hitLightning.gameObject.SetActive(true);
                hitLightning.Play();
                hitSpark.gameObject.SetActive(true);
                hitSpark.Play();

                transform.position = other.ClosestPoint(sticksPoint.position);
            }
        }
    }

    /// <summary>
    /// ナイフを投げる
    /// </summary>
    /// <param name="moveSpeed">投げる速度</param>
    /// <param name="moveDirection">投げる方向</param>
    public void Throw(float moveSpeed, Vector3 moveDirection)
    {
        throws = true;

        rb.isKinematic = false;
        rb.AddForce(moveSpeed * moveDirection, ForceMode.Impulse);
    }

    /// <summary>
    /// ナイフを戻す
    /// </summary>
    /// <param name="returnDuration">戻す時間</param>
    /// <param name="moveDirection">戻す方向</param>
    public void Return(float returnDuration, Vector3 moveDirection)
    {
        //  向きを移動と逆向きにする
        transform.forward = -moveDirection;

        //  ナイフを戻すときの設定
        returnBeginPosition = transform.position;
        this.returnDuration = returnDuration;
        returnRate = 0;

        throws = false;
        returns = true;
        Sticks = false;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// ナイフを手に持つ
    /// </summary>
    public void Hold()
    {
        throws = false;
        returns = false;
        Sticks = false;

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }
}
