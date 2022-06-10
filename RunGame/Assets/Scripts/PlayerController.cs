using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
  [SerializeField] float runSpeed;
  [SerializeField] float slidingMultiplier;
  [SerializeField] float jumpForce;
  [SerializeField] Vector3 gravity;
  [SerializeField] float groundRayRadius;
  [SerializeField] float groundRayLength;
  [SerializeField] LayerMask groundRayLayerMask;
  [SerializeField] Transform aimTarget;
  [SerializeField] CameraController cameraController;

  PlayerInput playerInput;
  InputAction move;
  InputAction look;
  InputAction jump;
  InputAction sliding;

  PlayerState currentPlayerState;
  Rigidbody rb;
  Quaternion cameraRotation;
  Transform mainCamera;

  public enum PlayerState
  {
    Default,
    Jump,
    Sliding,
  }
  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();

    //  InputActionのセット
    playerInput = GetComponent<PlayerInput>();
    move = playerInput.actions["Move"];
    look = playerInput.actions["Look"];
    jump = playerInput.actions["Jump"];
    sliding = playerInput.actions["Sliding"];

    mainCamera = Camera.main.transform;
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    //  カメラを基準にプレイヤーを動かす
    cameraRotation = Quaternion.AngleAxis(mainCamera.eulerAngles.y, Vector3.up);
    Vector2 moveInput = move.ReadValue<Vector2>();
    Vector3 moveDirection = cameraRotation * new Vector3(moveInput.x, 0, moveInput.y).normalized;
    float moveSpeed = runSpeed;

    switch (currentPlayerState)
    {
      case PlayerState.Default:

        if (IsGrounded())
        {
          if (jump.triggered)
          {
            SetPlayerState(PlayerState.Jump);
          }

          if (sliding.triggered)
          {
            SetPlayerState(PlayerState.Sliding);
          }
        }

        break;
      case PlayerState.Jump:
        //  着地したら状態を戻す
        if (IsGrounded())
        {
          SetPlayerState(PlayerState.Default);
        }

        break;
      case PlayerState.Sliding:
        moveSpeed = runSpeed * slidingMultiplier;

        break;
    }

    //  カメラの回転
    aimTarget.transform.rotation = Quaternion.Euler(cameraController.CameraRotate(aimTarget.transform, look.ReadValue<Vector2>()));

    //  移動速度のセット
    rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
  }

  private void FixedUpdate()
  {
    rb.AddForce(gravity, ForceMode.Acceleration);
  }

  /// <summary>
  /// プレイヤーの状態を変更する
  /// </summary>
  /// <param name="setState">変更する状態</param>
  public void SetPlayerState(PlayerState setState)
  {
    switch (setState)
    {
      case PlayerState.Default:

        break;
      case PlayerState.Jump:
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        break;
      case PlayerState.Sliding:

        break;
    }

    currentPlayerState = setState;
  }

  /// <summary>
  /// 接地判定
  /// </summary>
  /// <returns></returns>
  private bool IsGrounded()
  {
    bool isHit = false;
    if (Physics.SphereCast(aimTarget.transform.position, groundRayRadius, Vector3.down, out RaycastHit hit, groundRayLength, groundRayLayerMask.value))
    {
      if (hit.collider.CompareTag("Ground"))
      {
        isHit = true;
      }
    }
    return isHit;
  }
}
