using DigitalRuby.Tween;
using UnityEngine;

public class OptionsPanelController : MonoBehaviour
{
  private int m_CurrentTab;
  [SerializeField] private float m_SwitchDuration;
  [SerializeField] private CanvasGroup[] m_TabContents;

  public void SwitchTabTo(int tab)
  {
    if (m_CurrentTab == tab) {
      return;
    }

    var a = m_TabContents[m_CurrentTab];
    var b = m_TabContents[tab];

    a.interactable = false;
    b.gameObject.SetActive(true);
    b.interactable = false;

    TweenFactory.Tween("FadeA", 1.0f, 0.0f, m_SwitchDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      a.alpha = t.CurrentValue;
    }, t => {});

    TweenFactory.Tween("FadeB", 0.0f, 1.0f, m_SwitchDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      b.alpha = t.CurrentValue;
    }, t => {});

    m_CurrentTab = tab;
  }
}