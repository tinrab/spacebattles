using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizeText : MonoBehaviour
{
  [SerializeField]
  private string m_Key;
  [SerializeField]
  private bool m_UpperCase;
  [SerializeField]
  private string m_Before;
  [SerializeField]
  private string m_After;

  private void Start()
  {
    Localize();

    Destroy(this);
  }

  [ContextMenu("Localize")]
  public void Localize()
  {
    var text = GetComponent<Text>();
    var localized = Localization.GetText(m_Key);

    if (m_UpperCase) {
      text.text = m_Before + localized.ToUpperInvariant() + m_After;
    } else {
      text.text = m_Before + localized + m_After;
    }
  }
}