using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItemButtonHandler : MonoBehaviour
{
    [Header("References")]
    public string roomName;
    public void OnButtonPressed()
    {
        RoomManager.Instance.JoinRoomByName(roomName);
    }
}
