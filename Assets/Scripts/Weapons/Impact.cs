using UnityEngine;

public class Impact : MonoBehaviour
{
  private void Start()
  {
    Destroy(gameObject, 0.5f);
  }
}