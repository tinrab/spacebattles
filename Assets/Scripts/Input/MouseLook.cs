using UnityEngine;

public class MouseLook : MonoBehaviour
{
  [SerializeField] private float rotationSpeed;
  [SerializeField] private float dampingTime;
  private Vector3 targetAngles;
  private Vector3 followAngles;
  private Vector3 followVelocity;
  private Quaternion originalRotation;
  private CursorLockMode m_LockMode;

  private void Start()
  {
    originalRotation = transform.localRotation;
  }

  private void Update()
  {
    // we make initial calculations from the original local rotation
    transform.localRotation = originalRotation;

    // read input from mouse or mobile controls
    var inputH = Input.GetAxis("Mouse X");
    var inputV = Input.GetAxis("Mouse Y");

    // wrap values to avoid springing quickly the wrong way from positive to negative
    if (targetAngles.y > 180) {
      targetAngles.y -= 360;
      followAngles.y -= 360;
    }
    if (targetAngles.x > 180) {
      targetAngles.x -= 360;
      followAngles.x -= 360;
    }
    if (targetAngles.y < -180) {
      targetAngles.y += 360;
      followAngles.y += 360;
    }
    if (targetAngles.x < -180) {
      targetAngles.x += 360;
      followAngles.x += 360;
    }

    // with mouse input, we have direct control with no springback required.
    targetAngles.y += inputH * rotationSpeed;
    targetAngles.x += inputV * rotationSpeed;

    // clamp values to allowed range
    //targetAngles.y = Mathf.Clamp(targetAngles.y, -rotationRange.y * 0.5f, rotationRange.y * 0.5f);
    targetAngles.x = Mathf.Clamp(targetAngles.x, -90.0f, 90.0f);

    // smoothly interpolate current values to target angles
    followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, dampingTime);

    // update the actual gameobject's rotation
    transform.localRotation = originalRotation * Quaternion.Euler(-followAngles.x, followAngles.y, 0);

#if UNITY_EDITOR
    if (Input.GetKeyDown(KeyCode.Escape)) {
      m_LockMode = m_LockMode == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
    }

    Cursor.lockState = m_LockMode;
    Cursor.visible = CursorLockMode.Locked != m_LockMode;
#endif
  }
}