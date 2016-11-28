using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
  [SerializeField] protected Muzzle m_Muzzle;
  [SerializeField] public Impact m_ImpactPrefab;
  protected LayerMask m_TargetLayer;

  private void Awake()
  {
    m_ImpactPrefab.CreatePool(10);
    m_TargetLayer = LayerMask.NameToLayer("World") | LayerMask.NameToLayer("Enemy");
  }

  public abstract void Fire();
}
