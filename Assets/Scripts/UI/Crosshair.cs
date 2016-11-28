using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Image))]
public class Crosshair : MonoBehaviour
{
  private Image m_Image;

  public Sprite sprite
  {
    set
    {
      m_Image.sprite = value;
    }
  }

  private void Awake()
  {
    m_Image = GetComponent<Image>();
  }
}