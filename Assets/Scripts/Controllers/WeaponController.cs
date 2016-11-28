using UnityEngine;

public class WeaponController : Singleton<WeaponController>
{
  [SerializeField] private Weapon m_CurrentWeapon;
  private Weapon[] m_WeaponPrefabs;

  private void Start()
  {
    m_WeaponPrefabs = Resources.LoadAll<Weapon>("Prefabs/Weapons");
  }

  public void Fire()
  {
    m_CurrentWeapon.Fire();
  }
}