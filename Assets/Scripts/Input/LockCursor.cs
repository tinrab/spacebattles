using UnityEngine;

public class LockCursor : MonoBehaviour
{
  private bool m_IsLocked;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      if (m_IsLocked) {
        m_IsLocked = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
      } else {
        m_IsLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      }
    }
  }
}