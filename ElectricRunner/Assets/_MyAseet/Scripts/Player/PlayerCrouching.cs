using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouching : MonoBehaviour
{
    [Header("しゃがみの設定")]
    [SerializeField, Tooltip("移動速度倍率")] private float crouchingMoveMultiplier;

    [Header("スライディングの設定")]
    [SerializeField, Tooltip("移動速度倍率")] private float slidingMoveMultiplier;
    [SerializeField, Tooltip("スライディングの助走時間")] private float slidingThreshold;
    [SerializeField, Tooltip("スライディングしている最大時間")] private float slidingMaxTime;
    [SerializeField, Tooltip("スライディングした時に基本速度を上げる倍率"), Range(1, 2)] private float runSpeedUpMultiplier = 1.1f;

    [Header("カメラの設定")]
    [SerializeField, Tooltip("カメラの上下移動速度")] private float cameraUpDownSpeed;
    [SerializeField, Tooltip("カメラの下がる座標")] private Vector3 cameraDownPosition;
    [SerializeField, Tooltip("カメラのターゲット")] private Transform aimTarget;

    private PlayerController playerController;

    private float slidingTime;
    private float forwardMoveTime;
    private bool canSliding;

    //  カメラ移動の設定
    private Vector3 defaultCameraPosition;
    private bool isCameraMoveDown;
    private bool isCameraMoveUp;

    //  しゃがんでいる時のコライダーの設定
    private CapsuleCollider capsuleCollider;
    private Vector3 colliderDefaultCenter;
    private Vector3 colliderPostCenter;
    private float colliderDefaultHeight;
    private float colliderPostHeight;

    public Vector3 MoveDirection { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        colliderDefaultCenter = capsuleCollider.center;
        colliderDefaultHeight = capsuleCollider.height;

        //  しゃがんでいる時のコライダーの高さをカメラの高さに合わせる
        colliderPostCenter = cameraDownPosition / 2;
        colliderPostHeight = cameraDownPosition.y; ;

        defaultCameraPosition = aimTarget.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //  カメラの位置を下げる
        if (isCameraMoveDown)
        {
            aimTarget.localPosition = Vector3.Lerp(aimTarget.localPosition, cameraDownPosition, cameraUpDownSpeed * Time.deltaTime);
        }
        //  カメラの位置を上げる
        else if (isCameraMoveUp)
        {
            aimTarget.localPosition = Vector3.Lerp(aimTarget.localPosition, defaultCameraPosition, cameraUpDownSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 前進時間をリセットする
    /// </summary>
    public void ResetForwardMoveTime()
    {
        forwardMoveTime = 0;
        canSliding = false;
    }

    /// <summary>
    /// 前進している時間の計測
    /// </summary>
    /// <param name="movesForward">前進しているか</param>
    public void AddForwardMoveTime(bool movesForward)
    {
        //  前進していたら足す
        if (movesForward)
        {
            forwardMoveTime += Time.deltaTime;
        }
        //  前進していなかったら計測時間をリセットする
        else
        {
            ResetForwardMoveTime();
        }

        //  閾値を超えたらスライディングできる
        if (forwardMoveTime >= slidingThreshold)
        {
            canSliding = true;
        }
    }

    /// <summary>
    /// スライディングしている時間の計測
    /// </summary>
    public void AddSlidingTime()
    {
        slidingTime += Time.deltaTime;

        if (slidingTime >= slidingMaxTime)
        {
            playerController.SetPlayerState(PlayerController.PlayerState.Crouch);
        }
    }

    /// <summary>
    /// しゃがみトリガーが押された時
    /// </summary>
    public void OnPushCrouchingTrigger()
    {
        isCameraMoveDown = true;
        isCameraMoveUp = false;

        capsuleCollider.center = colliderPostCenter;
        capsuleCollider.height = colliderPostHeight;

        //  スライディング可能状態ならば、スライディング状態に遷移
        if (canSliding)
        {
            MoveDirection = transform.forward;
            playerController.RunSpeedUp(runSpeedUpMultiplier);
            playerController.SetPlayerState(PlayerController.PlayerState.Sliding);
        }
        else
        {
            //  しゃがみ状態に遷移
            playerController.SetPlayerState(PlayerController.PlayerState.Crouch);
        }
    }

    /// <summary>
    ///  しゃがみトリガーが離された時
    /// </summary>
    public void OnReleaseCrouchingTrigger()
    {
        isCameraMoveDown = false;
        isCameraMoveUp = true;

        capsuleCollider.center = colliderDefaultCenter;
        capsuleCollider.height = colliderDefaultHeight;

        slidingTime = 0;

        playerController.SetPlayerState(PlayerController.PlayerState.Default);
    }

    /// <summary>
    /// しゃがみ時の移動速度を取得する
    /// </summary>
    /// <param name="moveSpeed">現在の移動速度</param>
    /// <returns>しゃがみ時の移動速度</returns>
    public float GetCrouchingMoveSpeed(float moveSpeed)
    {
        return moveSpeed * crouchingMoveMultiplier;
    }

    /// <summary>
    /// スライディング時の移動速度を取得する
    /// </summary>
    /// <param name="moveSpeed">現在の移動速度</param>
    /// <returns>スライディング時の移動速度</returns>
    public float GetSlidingMoveSpeed(float moveSpeed)
    {
        return moveSpeed * slidingMoveMultiplier;
    }
}
