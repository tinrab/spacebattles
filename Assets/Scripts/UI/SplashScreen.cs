using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
  private void Start()
  {
    Invoke("SwitchScene", 1.0f);
  }

  private void SwitchScene()
  {
    SceneManager.LoadSceneAsync("Menu");
  }
}