using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] Vector2 rotationSpeed;

  [SerializeField] float minCameraAngle;
  [SerializeField] float maxCameraAngle;

  // Start is called before the first frame update
  void Start()
  {

  }


  public Vector3 CameraRotateHorizontal(Transform target, Vector2 lookInput)
  {
    Vector3 targetEulerAngles = target.localEulerAngles;

    targetEulerAngles.y += lookInput.x * rotationSpeed.x * Time.deltaTime;

    return targetEulerAngles;
  }

  public Vector3 CameraRotateVertical(Transform target, Vector2 lookInput)
  {
    Vector3 targetEulerAngles = target.localEulerAngles;

    targetEulerAngles.x += lookInput.y * -rotationSpeed.y * Time.deltaTime;
    targetEulerAngles.y = 0;
    targetEulerAngles.z = 0;

    if (targetEulerAngles.x > 180f)
    {
      targetEulerAngles.x -= 360f;
    }

    targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);

    return targetEulerAngles;
  }
}
