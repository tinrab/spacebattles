using UnityEngine;

public class MouseLook : MonoBehaviour
{
  private Quaternion m_CameraTargetHorizontalRotation;
  private Quaternion m_CameraTargetVerticalRotation;
  private CursorLockMode m_LockMode;
  [SerializeField] private Vector2 m_Sensitivity;
  private bool m_Smoothing;
  private float m_SmoothTime;
  [SerializeField] private Transform m_Vertical;
  [SerializeField] private Transform m_Horizontal;

  private void Start()
  {
    m_CameraTargetHorizontalRotation = m_Horizontal.localRotation;
    m_CameraTargetVerticalRotation = m_Vertical.localRotation;
  }

  private void Update()
  {
    if (!new Rect(1, 1, Screen.width - 2, Screen.height - 2).Contains(Input.mousePosition)) {
      return;
    }

    var yr = Input.GetAxis("Mouse X") * m_Sensitivity.x * 2.0f;
    var xr = Input.GetAxis("Mouse Y") * m_Sensitivity.y * 2.0f;

    m_CameraTargetHorizontalRotation *= Quaternion.Euler(0.0f, yr, 0.0f);

    m_CameraTargetVerticalRotation *= Quaternion.Euler(-xr, 0.0f, 0.0f);
    m_CameraTargetVerticalRotation = ClampRotationAroundX(m_CameraTargetVerticalRotation);

    if (m_Smoothing) {
      m_Horizontal.localRotation = Quaternion.Slerp(m_Horizontal.localRotation, m_CameraTargetHorizontalRotation, m_SmoothTime * Time.deltaTime);
      m_Vertical.localRotation = Quaternion.Slerp(m_Vertical.localRotation, m_CameraTargetVerticalRotation, m_SmoothTime * Time.deltaTime);
    } else {
      m_Horizontal.localRotation = m_CameraTargetHorizontalRotation;
      m_Vertical.localRotation = m_CameraTargetVerticalRotation;
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