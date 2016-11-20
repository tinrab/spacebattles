using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : Singleton<Tooltip>
{
  [SerializeField]
  private RectTransform m_Widget;
  [SerializeField]
  private Text m_Text;
  private CanvasGroup m_WidgetGroup;
  private bool m_IsWidgetActive;
  [SerializeField]
  private float m_FadeDuration;
  private RectTransform m_Canvas;
  private Camera m_UICamera;
  [SerializeField]
  private float m_VerticalOffset;
  [SerializeField]
  private float m_BorderPadding;

  private void Start()
  {
    m_WidgetGroup = m_Widget.GetComponent<CanvasGroup>();
    var canvas = FindObjectOfType<Canvas>();
    m_Canvas = canvas.GetComponent<RectTransform>();
    m_UICamera = canvas.worldCamera;
  }

  private void Update()
  {
    if (m_IsWidgetActive) {
      Vector2 pos;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas, Input.mousePosition, m_UICamera, out pos);
      pos = m_Canvas.TransformPoint(pos);
      pos.y += m_VerticalOffset;

      var hs = m_Widget.rect.size / 2.0f;
      var canvasSize = m_Canvas.rect.size;

      if (pos.x - hs.x < m_BorderPadding) {
        pos.x = hs.x + m_BorderPadding;
      } else if (pos.x + hs.x >= canvasSize.x - m_BorderPadding) {
        pos.x = canvasSize.x - hs.x - m_BorderPadding;
      }

      if (pos.y - hs.y < m_BorderPadding) {
        pos.y = hs.y + m_BorderPadding;
      } else if (pos.y + hs.y >= canvasSize.y - m_BorderPadding) {
        pos.y = canvasSize.y - hs.y - m_BorderPadding;
      }

      m_Widget.position = pos;
    }
  }

  public void OnPointerEnter(TooltipContext context, PointerEventData eventData)
  {
    m_IsWidgetActive = true;
    m_Text.text = Localization.GetText(context.textKey);
    m_Widget.gameObject.SetActive(true);

    TweenFactory.RemoveTweenKey("HideTooltip", TweenStopBehavior.DoNotModify);
    TweenFactory.Tween("ShowTooltip", m_WidgetGroup.alpha, 1.0f, m_FadeDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      m_WidgetGroup.alpha = t.CurrentValue;
    }, t => {});
  }

  public void OnPointerExit(TooltipContext context, PointerEventData eventData)
  {
    TweenFactory.RemoveTweenKey("ShowTooltip", TweenStopBehavior.DoNotModify);
    TweenFactory.Tween("HideTooltip", m_WidgetGroup.alpha, 0.0f, m_FadeDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      m_WidgetGroup.alpha = t.CurrentValue;
    }, t =>
    {
      m_Widget.gameObject.SetActive(false);
      m_IsWidgetActive = false;
    });
  }
}