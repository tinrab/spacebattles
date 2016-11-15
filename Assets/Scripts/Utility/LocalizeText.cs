using UnityEngine;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour
{
  [SerializeField]
  private Text m_Text;
  [SerializeField]
  private string m_Key;
  [SerializeField]
  private bool m_UpperCase;

  private void Start()
  {
    Localize();

    Destroy(this);
  }

  [ContextMenu("Localize")]
  public void Localize()
  {
    m_Text.text = Localization.GetText(m_Key);

    if (m_UpperCase) {
      m_Text.text = m_Text.text.ToUpperInvariant();
    }
  }
}