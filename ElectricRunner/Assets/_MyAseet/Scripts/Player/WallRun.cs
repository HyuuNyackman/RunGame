using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField, Tooltip("カメラの回転角")] private float cameraTilt;
    [SerializeField, Tooltip("カメラの回転スピード")] private float cameraTiltSpeed;
    [SerializeField, Tooltip("壁判定の原点")] private Transform rayOrigin;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask wallLayer;

    public Vector3 JumpDirection { get; private set; }
    public bool RunLeft { get; private set; }
    public bool RunRight { get; private set; }
    public float Tilt { get; private set; }
    public float GetJumpForce => jumpForce;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rayOrigin.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        Debug.DrawRay(rayOrigin.position, -rayOrigin.right * rayLength, Color.red);
        Debug.DrawRay(rayOrigin.position, rayOrigin.right * rayLength, Color.red);

        if (RunLeft)
        {
            Tilt = Mathf.Lerp(Tilt, -cameraTilt, cameraTiltSpeed * Time.deltaTime);
        }
        else if (RunRight)
        {
            Tilt = Mathf.Lerp(Tilt, cameraTilt, cameraTiltSpeed * Time.deltaTime);
        }
        else
        {
            Tilt = Mathf.Lerp(Tilt, 0, cameraTiltSpeed * Time.deltaTime);
        }
    }

    private bool ExistsWallLeft()
    {
        bool existWall = false;

        if (Physics.Raycast(rayOrigin.position, -rayOrigin.right, out RaycastHit wallLeftHit, rayLength, wallLayer.value))
        {
            if (wallLeftHit.collider.CompareTag("Wall"))
            {
                existWall = true;
            }
        }

        if (existWall)
        {
            RunLeft = true;
            JumpDirection = (transform.up + wallLeftHit.normal).normalized;
        }

        return existWall;
    }

    private bool ExistsWallRight()
    {
        bool existWall = false;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.right, out RaycastHit wallRightHit, rayLength, wallLayer.value))
        {
            if (wallRightHit.collider.CompareTag("Wall"))
            {
                existWall = true;
            }
        }
        if (existWall)
        {
            RunRight = true;
            JumpDirection = (transform.up + wallRightHit.normal).normalized;
        }

        return existWall;
    }

    public bool ExistsWall()
    {
        if (RunLeft)
        {
            return ExistsWallLeft();
        }
        else if (RunRight)
        {
            return ExistsWallRight();
        }
        else
        {
            return ExistsWallLeft() || ExistsWallRight();
        }
    }

    public void EndWallRun()
    {
        RunLeft = false;
        RunRight = false;
    }
}
