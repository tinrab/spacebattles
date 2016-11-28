using UnityEditor;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
  public void OnQuitClick()
  {
    Dialog.instance.Show(Localization.GetText("QuitDialog"), new[] {
      Localization.GetText("Yes"),
      Localization.GetText("No")
    }, button =>
    {
      if (button == 0) {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
          Application.Quit();
#endif
      }
    });
  }
}