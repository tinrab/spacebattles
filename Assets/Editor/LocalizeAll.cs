using UnityEditor;
using UnityEngine;

public class LocalizeAll : MonoBehaviour
{
  [MenuItem("Tools/Localize All")]
  public static void Perform()
  {
    var texts = FindObjectsOfType<LocalizeText>();

    for (var i = 0; i < texts.Length; i++) {
      texts[i].Localize();
    }
  }
}