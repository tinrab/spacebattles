using UnityEngine;

public class MouseLook : MonoBehaviour
{
  [SerializeField]
  private Transform m_View;
  [SerializeField]
  private Transform m_Camera;
  [SerializeField]
  private Vector2 m_Sensitivity;
  private bool m_Smoothing;
  private float m_SmoothTime;
  private Quaternion m_CameraTargetHorizontalRotation;
  private Quaternion m_CameraTargetVerticalRotation;
  private CursorLockMode m_LockMode;

  private void Start()
  {
    m_CameraTargetHorizontalRotation = m_View.localRotation;
    m_CameraTargetVerticalRotation = m_Camera.localRotation;
  }

  private void Update()
  {
    if (!new Rect(1, 1, Screen.width - 2, Screen.height - 2).Contains(Input.mousePosition)) {
      return;
    }

    var yr = Input.GetAxis("Mouse X") * m_Sensitivity.x;
    var xr = Input.GetAxis("Mouse Y") * m_Sensitivity.y;

    m_CameraTargetHorizontalRotation *= Quaternion.Euler(0.0f, yr, 0.0f);

    m_CameraTargetVerticalRotation *= Quaternion.Euler(-xr, 0.0f, 0.0f);
    m_CameraTargetVerticalRotation = ClampRotationAroundX(m_CameraTargetVerticalRotation);

    if (m_Smoothing) {
      m_View.localRotation = Quaternion.Slerp(m_View.localRotation, m_CameraTargetHorizontalRotation, m_SmoothTime * Time.deltaTime);
      m_Camera.localRotation = Quaternion.Slerp(m_Camera.localRotation, m_CameraTargetVerticalRotation, m_SmoothTime * Time.deltaTime);
    } else {
      m_View.localRotation = m_CameraTargetHorizontalRotation;
      m_Camera.localRotation = m_CameraTargetVerticalRotation;
    }

#if UNITY_EDITOR
    if (Input.GetKeyDown(KeyCode.Escape)) {
      m_LockMode = m_LockMode == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
    }

    Cursor.lockState = m_LockMode;
    Cursor.visible = CursorLockMode.Locked != m_LockMode;
#endif
  }

  private Quaternion ClampRotationAroundX(Quaternion q)
  {
    q.x /= q.w;
    q.y /= q.w;
    q.z /= q.w;
    q.w = 1.0f;

    var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

    angleX = Mathf.Clamp(angleX, -90.0f, 90.0f);

    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

    return q;
  }
}