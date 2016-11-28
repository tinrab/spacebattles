using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

internal interface IWindowEventHandler
{
  void OnWindowShow();
  void OnWindowHide();
}

public class Window : MonoBehaviour
{
  private IWindowEventHandler[] m_Handlers;
  [SerializeField] private Image m_Overlay;
  private Color m_OverlayColor;
  [SerializeField] private CanvasGroup m_Widget;

  private void Awake()
  {
    m_OverlayColor = new Color(0.0f, 0.0f, 0.0f, 0.75f);
    m_Overlay.raycastTarget = false;
    m_Overlay.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    m_Widget.alpha = 0.0f;
    m_Widget.interactable = false;
    m_Widget.blocksRaycasts = false;

    m_Handlers = GetComponents<IWindowEventHandler>();
  }

  public void Show()
  {
    m_Overlay.raycastTarget = true;
    TweenFactory.Tween("ShowWindow", 0.0f, 1.0f, 0.2f, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      var x = t.CurrentValue;
      m_OverlayColor.a = x * 0.75f;
      m_Overlay.color = m_OverlayColor;
      m_Widget.alpha = x;
      m_Widget.transform.localScale = new Vector3(x, x, 1.0f);
    }, t =>
    {
      m_Widget.interactable = true;
      m_Widget.blocksRaycasts = true;

      for (var i = 0; i < m_Handlers.Length; i++) {
        m_Handlers[i].OnWindowShow();
      }
    });
  }

  public void Hide()
  {
    m_Widget.interactable = false;
    m_Widget.blocksRaycasts = false;
    TweenFactory.Tween("HideWindow", 1.0f, 0.0f, 0.2f, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      var x = t.CurrentValue;
      m_OverlayColor.a = x * 0.75f;
      m_Overlay.color = m_OverlayColor;
      m_Widget.alpha = x;
      m_Widget.transform.localScale = new Vector3(x, x, 1.0f);
    }, t =>
    {
      m_Overlay.raycastTarget = false;

      for (var i = 0; i < m_Handlers.Length; i++) {
        m_Handlers[i].OnWindowHide();
      }
    });
  }

  [ContextMenu("Hide")]
  private void HideInEditor()
  {
    m_Widget.interactable = false;
    m_Widget.blocksRaycasts = false;
    m_Overlay.raycastTarget = false;
    m_Widget.alpha = 0.0f;
    m_Widget.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
  }

  [ContextMenu("Show")]
  private void ShowInEditor()
  {
    m_Widget.interactable = true;
    m_Widget.blocksRaycasts = true;
    m_Overlay.raycastTarget = true;
    m_Widget.alpha = 1.0f;
    m_Widget.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
  }
}