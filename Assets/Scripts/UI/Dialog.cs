using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialog : Singleton<Dialog>
{
  [SerializeField]
  private GameObject m_Body;
  [SerializeField]
  private Text m_Text;
  [SerializeField]
  private Button[] m_Buttons;

  private UnityAction<int> m_OnClick;

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

    m_Body.SetActive(true);
  }

  public void OnButtonClick(int button)
  {
    m_OnClick.Invoke(button);
    m_Body.SetActive(false);
    m_OnClick = null;
  }
}