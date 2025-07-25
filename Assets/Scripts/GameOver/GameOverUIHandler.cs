using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameOverUIHandler : MonoBehaviour
{

    [Header("Events")]
    [SerializeField] private GameEvent leaveToLobbyEvent;



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
