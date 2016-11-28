using UnityEngine;
using UnityEngine.UI;

public class TestShowDialog : MonoBehaviour
{
  [SerializeField] private Text m_ResultText;

  public void ShowDialog()
  {
    Dialog.instance.Show("something", new[] {
      "YES",
      "NO"
    }, choice =>
    {
      m_ResultText.text = "clicked: " + (choice == 0 ? "YES" : "NO");
    });
  }
}