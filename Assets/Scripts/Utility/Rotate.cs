using UnityEngine;

public class Rotate : MonoBehaviour
{
  [SerializeField]
  private Vector3 m_EulerAngles;
  [SerializeField]
  private Space m_RelativeTo;

  private void Update()
  {
    transform.Rotate(m_EulerAngles * Time.deltaTime, m_RelativeTo);
  }
}