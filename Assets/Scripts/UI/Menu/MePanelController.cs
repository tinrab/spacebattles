using UnityEngine;
using UnityEngine.UI;

public class MePanelController : MonoBehaviour
{
  [SerializeField]
  private Image m_UserAvatar;
  [SerializeField]
  private Text m_UserName;

  private void Start()
  {
    var social = SocialService.instance;

    social.GetUserAvatar(result =>
    {
      if (result.success) {
        // images are upside down
        m_UserAvatar.sprite = result.value;
        m_UserAvatar.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
      } else {
        m_UserAvatar.transform.localScale = Vector3.one;
      }
    });
    m_UserName.text = social.GetUserName();
  }
}