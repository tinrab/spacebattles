using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipContext : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [SerializeField]
  private string m_TextKey;

  public string textKey
  {
    get {
      return m_TextKey;
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    Tooltip.instance.OnPointerEnter(this, eventData);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    Tooltip.instance.OnPointerExit(this, eventData);
  }
}