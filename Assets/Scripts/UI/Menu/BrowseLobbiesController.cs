using UnityEngine;

public class BrowseLobbiesController : MonoBehaviour, IWindowEventHandler
{
  private SocialService m_Social;
  public void OnWindowShow() {}
  public void OnWindowHide() {}

  private void Start()
  {
    m_Social = SocialService.instance;
  }
}