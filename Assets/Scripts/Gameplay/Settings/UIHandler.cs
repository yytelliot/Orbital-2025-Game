using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject settingScreen;

    [Header("Events")]
    [SerializeField] private GameEvent leaveToLobbyEvent;
    private bool settingScreenActive = false;
    void Update()
    {
        if (InputManager.GetKeyDown("settings"))
        {
            if (settingScreenActive)
            {
                SoundManagerTechnican.PlaySound(SoundType.SETTINGCLOSE, 0.5f);
                settingScreen.SetActive(false);
                settingScreenActive = false;
            }
            else
            {
                SoundManagerTechnican.PlaySound(SoundType.SETTINGOPEN, 0.5f);
                settingScreen.SetActive(true);
                settingScreenActive = true;
            }
        }
    }

    #region ("Exit To Lobby")

    public void DisconnectPlayer()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object role);
        leaveToLobbyEvent.RaiseNetworked(this, role.ToString());
        StartCoroutine(DisconnectAndLoad());

    }

    public void DisconnectOtherPlayer(Component sender, object data)
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object role);

        if (role.ToString() != data.ToString())
        {
            StartCoroutine(DisconnectAndLoad());
        }

    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
        {
            { "PlayerRole", null },
            { "HasConfirmed", null }
        });

        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;

        if (RoleSelectionManager.Instance != null)
        {
            Destroy(RoleSelectionManager.Instance.gameObject);
        }

        if (CrossSceneNetworkManager.Instance != null)
        {
            Destroy(CrossSceneNetworkManager.Instance.gameObject);
        }


        SceneManager.LoadScene("Lobby");
    }
    
    #endregion
}
