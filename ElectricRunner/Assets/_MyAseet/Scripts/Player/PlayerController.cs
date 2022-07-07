using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("基本設定")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField, Tooltip("ジャンプ時の移動のしやすさ")] private float jumpMoveForceMultiplier;
    [SerializeField] private Vector3 gravity;
    [SerializeField, Header("カメラのターゲット")] private Transform aimTarget;

    [Header("接地判定の設定")]
    [SerializeField] private float groundRayRadius;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField, Header("壁判定のディレイ")] private float wallDetectionDelay;

    [Header("スクリプト")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerCrouching playerCrouching;
    [SerializeField] private WallRun wallRun;
    [SerializeField] private KnifeShot knifeShot;

    private PlayerInput playerInput;
    private InputAction move;
    private InputAction look;
    private InputAction jump;
    private InputAction crouch;
    private InputAction throwKnife;

    private Rigidbody rb;
    private Animator animator;
    private Camera mainCamera;
    private float baseRunSpeed;
    private bool useGravity;
    private bool isGrounded;
    private bool canWallRun;


    private float moveSpeed;
    private Vector3 moveDirection;

    public bool HoldHand = true;

    public enum PlayerState
    {
        Default,    //  通常
        Crouch,     //  しゃがみ
        Sliding,    //  スライディング
        WallRun,    //  壁走り
    }

    public PlayerState CurrentPlayerState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //  InputActionをセットする
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        look = playerInput.actions["Look"];
        jump = playerInput.actions["Jump"];
        crouch = playerInput.actions["Crouch"];
        throwKnife = playerInput.actions["ThrowKnife"];

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        mainCamera = Camera.main;
        baseRunSpeed = runSpeed;
        useGravity = true;
        canWallRun = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //  プレイヤーの入力を取得
        Vector2 moveInput = move.ReadValue<Vector2>();
        Vector2 lookInput = look.ReadValue<Vector2>();

        //  カメラを基準にプレイヤーを動かす
        Quaternion cameraRotationY = Quaternion.AngleAxis(mainCamera.transform.eulerAngles.y, Vector3.up);
        moveDirection = cameraRotationY * new Vector3(moveInput.x, 0, moveInput.y).normalized;

        moveSpeed = runSpeed;
        isGrounded = IsGrounded();

        switch (CurrentPlayerState)
        {
            //  通常時の処理
            case PlayerState.Default:

                //  地面に接していればジャンプとスライディングができる
                if (isGrounded)
                {
                    if (jump.triggered)
                    {
                        Jump(Vector3.up, jumpForce);
                    }

                    if (crouch.triggered)
                    {
                        playerCrouching.OnPushCrouchingTrigger();
                    }
                }
                else
                {
                    if (canWallRun && moveInput.y > 0.1f && wallRun.ExistsWall())
                    {
                        SetPlayerState(PlayerState.WallRun);
                    }
                }

                //  前進している時間を計測する
                playerCrouching.AddForwardMoveTime(moveInput.y > 0);

                break;

            //  しゃがみ時の処理
            case PlayerState.Crouch:

                //  移動速度をしゃがみ時の速度にする
                moveSpeed = playerCrouching.GetCrouchingMoveSpeed(runSpeed);

                //  しゃがみの入力がなくなったら立ち上がる
                if (!crouch.IsPressed())
                {
                    playerCrouching.OnReleaseCrouchingTrigger();
                }

                //  地面に接していたらジャンプできる
                if (isGrounded)
                {
                    if (jump.triggered)
                    {
                        Jump(Vector3.up, jumpForce);
                    }
                }

                break;

            //  スライディング時の処理
            case PlayerState.Sliding:

                //  移動方向をスライディングし始めた時の方向にする
                moveDirection = playerCrouching.MoveDirection;

                //  移動速度をスライディング時の速度にする
                moveSpeed = playerCrouching.GetSlidingMoveSpeed(runSpeed);

                //  スライディングしている時間を計測する
                playerCrouching.AddSlidingTime();

                //  しゃがみの入力がなくなったら立ち上がる
                if (!crouch.IsPressed())
                {
                    playerCrouching.OnReleaseCrouchingTrigger();
                }

                //  前進しなくなったらしゃがみに遷移
                if (moveInput.y <= 0)
                {
                    SetPlayerState(PlayerState.Crouch);
                }

                //  地面に接していたらジャンプできる
                if (isGrounded)
                {
                    if (jump.triggered)
                    {
                        Jump(Vector3.up, jumpForce);
                    }
                }

                break;

            case PlayerState.WallRun:

                //  壁に触れなくなったか速度が一定以下なら壁走りをやめる
                if (!wallRun.ExistsWall() || moveInput.y <= 0.1f)
                {
                    SetPlayerState(PlayerState.Default);
                }

                if (jump.triggered)
                {
                    Jump(wallRun.JumpDirection, wallRun.GetJumpForce);
                }

                break;
        }

        //  ナイフを投げる処理
        if (throwKnife.triggered)
        {
            if (knifeShot.Shot)
            {
                knifeShot.ReCharge();
                HoldHand = true;
            }
            else
            {
                knifeShot.ThrowingKnife();
                HoldHand = false;
            }
        }

        //  カメラの垂直方向の回転
        aimTarget.transform.localRotation = Quaternion.Euler(cameraController.CameraRotateVertical(aimTarget.transform, lookInput));
        //  カメラの水平方向の回転
        transform.rotation = Quaternion.Euler(cameraController.CameraRotateHorizontal(transform, lookInput));
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.localEulerAngles.y, wallRun.Tilt));

        //  移動速度のセット
        if (isGrounded)
        {
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
        }
        else if (CurrentPlayerState == PlayerState.WallRun)
        {
            //  カメラのXを基準に上下にも移動させる
            Quaternion cameraRotationX = Quaternion.AngleAxis(mainCamera.transform.eulerAngles.x, transform.right);
            moveDirection = cameraRotationX * moveDirection.normalized;
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (useGravity)
        {
            //  重力をかける
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        //  地面に触れていなくて、壁走りじゃない時に慣性を付ける
        if (!isGrounded && CurrentPlayerState != PlayerState.WallRun)
        {
            rb.AddForce(jumpMoveForceMultiplier * (moveDirection * moveSpeed - new Vector3(rb.velocity.x, 0, rb.velocity.z)), ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// プレイヤーの状態を変更する
    /// </summary>
    /// <param name="setState">変更する状態</param>
    public void SetPlayerState(PlayerState setState)
    {
        //  変更前の状態を取得
        PlayerState previousPlayerState = CurrentPlayerState;

        //  変更後の状態を代入する
        CurrentPlayerState = setState;

        //  変更前の状態で遷移時に実行する処理
        switch (previousPlayerState)
        {
            case PlayerState.Default:
                break;

            case PlayerState.Crouch:
                break;

            case PlayerState.Sliding:

                break;

            case PlayerState.WallRun:

                useGravity = true;
                wallRun.EndWallRun();
                StopCoroutine(WallDetectionDelay());
                StartCoroutine(WallDetectionDelay());
                break;
        }

        //  変更後の状態で遷移時に実行する処理
        switch (setState)
        {
            case PlayerState.Default:

                break;

            case PlayerState.Crouch:

                break;

            case PlayerState.Sliding:

                break;

            case PlayerState.WallRun:
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                useGravity = false;
                break;
        }
    }

    public void RunSpeedUp(float SpeedUpMultiplier)
    {
        runSpeed += (SpeedUpMultiplier - 1) * baseRunSpeed;
    }

    private void Jump(Vector3 jumpDirection, float jumpForce)
    {
        //animator.SetTrigger(PlayerState.Jump.ToString());
        rb.AddForce(jumpDirection * jumpForce, ForceMode.VelocityChange);

        StopCoroutine(WallDetectionDelay());
        StartCoroutine(WallDetectionDelay());
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns>地面に接しているか</returns>
    private bool IsGrounded()
    {
        float rayLength = aimTarget.localPosition.y;

        if (Physics.SphereCast(aimTarget.position, groundRayRadius, Vector3.down, out RaycastHit hit, rayLength, groundLayer.value))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator WallDetectionDelay()
    {
        canWallRun = false;

        yield return new WaitForSeconds(wallDetectionDelay);

        canWallRun = true;
    }
}
