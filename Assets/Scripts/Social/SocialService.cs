using Steamworks;
using UnityEngine.Events;

public class SocialService : Singleton<SocialService>
{
  private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
  private UnityAction<ServiceResult<int>> m_NumberOfCurrentPlayersCallback;

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
    }
  }

  public string GetPersonaName()
  {
    return SteamFriends.GetPersonaName();
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

  public struct ServiceResult<T>
  {
    private bool success;
    private T value;

    public ServiceResult(bool success, T value = default(T))
    {
      this.success = success;
      this.value = value;
    }
  }
}