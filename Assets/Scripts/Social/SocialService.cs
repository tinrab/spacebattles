using Steamworks;
using UnityEngine;
using UnityEngine.Events;

public class SocialService : Singleton<SocialService>
{
  private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
  private UnityAction<ServiceResult<int>> m_NumberOfCurrentPlayersCallback;
  private Callback<AvatarImageLoaded_t> m_AvatarImageLoaded;
  private UnityAction<ServiceResult<Sprite>> m_AvatarImageLoadedCallback;

  public bool initialized
  {
    get {
      return SteamManager.Initialized;
    }
  }

  private void OnEnable()
  {
    if (initialized) {
      m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
      m_AvatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }
  }

  public string GetUserName()
  {
    return SteamFriends.GetPersonaName();
  }

  public void GetUserAvatar(UnityAction<ServiceResult<Sprite>> callback)
  {
    m_AvatarImageLoadedCallback = callback;
    var imageId = SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID());

    if (imageId != -1) {
      var sprite = LoadSprite(imageId);

      if (sprite == null) {
        callback(new ServiceResult<Sprite>(false));
      } else {
        callback(new ServiceResult<Sprite>(true, sprite));
      }
    }
  }

  public void GetNumberOfCurrentPlayers(UnityAction<ServiceResult<int>> callback)
  {
    var handle = SteamUserStats.GetNumberOfCurrentPlayers();
    m_NumberOfCurrentPlayers.Set(handle);
  }

  private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t callback, bool ioFailure)
  {
    if (callback.m_bSuccess != 1 || ioFailure) {
      m_NumberOfCurrentPlayersCallback(new ServiceResult<int>(false));
    } else {
      m_NumberOfCurrentPlayersCallback(new ServiceResult<int>(true, callback.m_cPlayers));
    }
  }

  private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
  {
    if (callback.m_iImage == 0) {
      m_AvatarImageLoadedCallback(new ServiceResult<Sprite>(false));
    } else {
      var sprite = LoadSprite(callback.m_iImage);

      if (sprite == null) {
        m_AvatarImageLoadedCallback(new ServiceResult<Sprite>(false));
      } else {
        m_AvatarImageLoadedCallback(new ServiceResult<Sprite>(true, sprite));
      }
    }
  }

  private Sprite LoadSprite(int imageId)
  {
    var texture = LoadTexture(imageId);
    if (texture == null) {
      return null;
    }

    var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
    var pivot = new Vector2(texture.width, texture.height) / 2.0f;

    return Sprite.Create(texture, rect, pivot);
  }

  private Texture2D LoadTexture(int imageId)
  {
    if (imageId == 0) {
      return null;
    }

    uint width, height;
    if (!SteamUtils.GetImageSize(imageId, out width, out height)) {
      return null;
    }

    if (width == 0 || height == 0) {
      return null;
    }

    var data = new byte[(int)(4 * width * height)];
    if (!SteamUtils.GetImageRGBA(imageId, data, data.Length)) {
      return null;
    }

    var texture = new Texture2D((int) width, (int) height, TextureFormat.RGBA32, false);
    texture.LoadRawTextureData(data);
    texture.Apply();

    return texture;
  }

  public struct ServiceResult<T>
  {
    public bool success;
    public T value;

    public ServiceResult(bool success, T value = default(T))
    {
      this.success = success;
      this.value = value;
    }
  }
}