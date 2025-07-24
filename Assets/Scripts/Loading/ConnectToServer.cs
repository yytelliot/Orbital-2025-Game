using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    private const string appId = "8c494d25-95c1-4a2a-a69d-fef185cade76";
    private const string gameVersion = "1.0";
    private const string fixedRegion = "asia";
    void Start()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = appId;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = fixedRegion;
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
