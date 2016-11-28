using UnityEngine;

public class Muzzle : MonoBehaviour
{
  [SerializeField] private ParticleSystem[] m_ParticleSystems;
  [SerializeField] private Light m_Light;

  private bool m_Flashing;
  private float m_Timer;

  public Vector3 origin
  {
    get
    {
      return transform.position;
    }
  }

  public Vector3 direction
  {
    get
    {
      return transform.forward;
    }
  }

  public void Flash()
  {
    for (int i = 0; i < m_ParticleSystems.Length; i++) {
      m_ParticleSystems[i].Play();
    }

    m_Flashing = true;
  }

  private void Update()
  {
    if (m_Flashing) {
      m_Timer += Time.deltaTime;

      if (m_Light) {
        m_Light.intensity = 1.0f - 0.2f / m_Timer;
      }

      if (m_Timer >= 0.2f) {
        m_Timer = 0.0f;
        m_Flashing = false;
      }
    }
  }

}