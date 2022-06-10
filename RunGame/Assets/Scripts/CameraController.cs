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


  public Vector3 CameraRotate(Transform target, Vector2 lookInput)
  {
    Vector3 targetEulerAngles = target.rotation.eulerAngles;

    targetEulerAngles.y += lookInput.x * rotationSpeed.x * Time.deltaTime;
    targetEulerAngles.x += lookInput.y * -rotationSpeed.y * Time.deltaTime;

    if (targetEulerAngles.x > 180f)
    {
      targetEulerAngles.x -= 360f;
    }

    targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);
    targetEulerAngles = new Vector3(targetEulerAngles.x, targetEulerAngles.y, 0);


    return targetEulerAngles;
  }
}
