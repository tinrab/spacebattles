using UnityEngine;

public class RayWeapon : Weapon
{
  public override void Fire()
  {
    var origin = m_Muzzle.origin;
    var direction = m_Muzzle.direction;

    m_Muzzle.Flash();

    RaycastHit hit;
    if (Physics.Raycast(origin, direction, out hit, 1000.0f, m_TargetLayer)) {
      var target = hit.collider.GetComponent<ITarget>();
      if (target != null) {
        target.OnShot();
      }

      m_ImpactPrefab.Spawn(hit.point, Quaternion.LookRotation(hit.normal));
    }
  }
}