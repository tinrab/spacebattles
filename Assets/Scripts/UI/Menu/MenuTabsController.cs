﻿using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

public class MenuTabsController : MonoBehaviour
{
  private static readonly Vector2 s_FarLeft = new Vector2(-1280.0f, 0.0f);
  private static readonly Vector2 s_FarRight = new Vector2(1280.0f, 0.0f);
  [SerializeField] private CanvasGroup[] m_CanvasGroups;
  private int m_CurrentTab;
  [SerializeField] private float m_SwitchDuration;
  [SerializeField] private RectTransform[] m_TabContents;
  [SerializeField] private Toggle[] m_Tabs;

  private void Start()
  {
    m_CanvasGroups = new CanvasGroup[m_TabContents.Length];
    for (var i = 0; i < m_CanvasGroups.Length; i++) {
      m_CanvasGroups[i] = m_TabContents[i].GetComponent<CanvasGroup>();
    }
  }

  public void SwitchTabTo(int tab)
  {
    if (m_CurrentTab == tab) {
      return;
    }

    if (tab < 2 || tab > 4) {
      for (var i = 0; i < m_Tabs.Length; i++) {
        m_Tabs[i].isOn = false;
      }
    }

    var ta = m_TabContents[m_CurrentTab];
    var tb = m_TabContents[tab];
    var ga = m_CanvasGroups[m_CurrentTab];
    var gb = m_CanvasGroups[tab];

    var targetPositionA = default(Vector2);
    var startPositionB = default(Vector2);

    if (tab < m_CurrentTab) {
      targetPositionA = s_FarRight;
      startPositionB = s_FarLeft;
    } else {
      targetPositionA = s_FarLeft;
      startPositionB = s_FarRight;
    }

    ga.interactable = false;
    tb.gameObject.SetActive(true);
    gb.interactable = false;

    TweenFactory.Tween("MoveA", Vector2.zero, targetPositionA, m_SwitchDuration, TweenScaleFunctions.CubicEaseInOut,
      t =>
      {
        ta.anchoredPosition = t.CurrentValue;
      }, t =>
      {
        ta.gameObject.SetActive(false);
      });
    TweenFactory.Tween("FadeA", 1.0f, 0.0f, m_SwitchDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      ga.alpha = t.CurrentValue;
    }, t => {});

    TweenFactory.Tween("MoveB", startPositionB, Vector2.zero, m_SwitchDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      tb.anchoredPosition = t.CurrentValue;
    }, t =>
    {
      gb.interactable = true;
    });
    TweenFactory.Tween("FadeB", 0.0f, 1.0f, m_SwitchDuration, TweenScaleFunctions.CubicEaseInOut, t =>
    {
      gb.alpha = t.CurrentValue;
    }, t => {});

    m_CurrentTab = tab;
  }
}