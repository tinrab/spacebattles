using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialog : Singleton<Dialog>
{
  [SerializeField] private Button[] m_Buttons;
  private UnityAction<int> m_OnClick;
  [SerializeField] private Image m_Overlay;
  private Color m_OverlayColor;
  [SerializeField] private Text m_Text;
  [SerializeField] private CanvasGroup m_Widget;

  private void Awake()
  {
    m_OverlayColor = new Color(0.0f, 0.0f, 0.0f, 0.75f);
    m_Overlay.raycastTarget = false;
    m_Overlay.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    m_Widget.alpha = 0.0f;
    m_Widget.interactable = false;
    m_Widget.blocksRaycasts = false;
  }

  public void Show(string text, string[] buttons, UnityAction<int> onClick)
  {
    m_Text.text = text;
    m_OnClick = onClick;

    for (var i = 0; i < m_Buttons.Length; i++) {
      var b = m_Buttons[i];

      if (i < buttons.Length) {
        b.gameObject.SetActive(true);
        b.GetComponentInChildren<Text>().text = buttons[i].ToUpperInvariant();
      } else {
        b.gameObject.SetActive(false);
      }
    }

    m_Overlay.raycastTarget = true;
    TweenFactory.Tween("ShowDialog", 0.0f, 1.0f, 0.2f, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      var x = t.CurrentValue;
      m_OverlayColor.a = x * 0.75f;
      m_Overlay.color = m_OverlayColor;
      m_Widget.alpha = x;
      m_Widget.transform.localScale = new Vector3(1.0f, x, 1.0f);
    }, t =>
    {
      m_Widget.interactable = true;
      m_Widget.blocksRaycasts = true;
    });
  }

  public void OnButtonClick(int button)
  {
    m_OnClick.Invoke(button);
    m_Widget.interactable = false;
    m_Widget.blocksRaycasts = false;
    TweenFactory.Tween("HideDialog", 1.0f, 0.0f, 0.2f, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      var x = t.CurrentValue;
      m_OverlayColor.a = x * 0.75f;
      m_Overlay.color = m_OverlayColor;
      m_Widget.alpha = x;
      m_Widget.transform.localScale = new Vector3(1.0f, x, 1.0f);
    }, t =>
    {
      m_Overlay.raycastTarget = false;
    });
    m_OnClick = null;
  }
}