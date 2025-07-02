using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveToLobby : MonoBehaviour
{
    [SerializeField] private UIHandler uIHandler;

    private void DisconnectPlayer()
    {
        uIHandler.DisconnectPlayer();
    }

}
