using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class RoomManager : MonoBehaviourPunCallbacks
{

    [Header("UI")]
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListPrefab;

    public static RoomManager Instance; //Singleton
    public GameObject createAndJoinRoomsGameObject;
    public CreateAndJoinRooms createAndJoinRooms;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(AutoRefresh());
    }


    IEnumerator AutoRefresh()
    {
        while (true)
        {
            Debug.Log("a");
            RefreshRoomList();
            yield return new WaitForSeconds(5);
        }
        
    }

    IEnumerator Start()
    {
        //Precautions
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }
    

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public void RefreshRoomList()
    {
        Debug.Log("Refreshing room list...");
        cachedRoomList.Clear();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby(); 
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (cachedRoomList.Count <= 0)
        {
            cachedRoomList = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < cachedRoomList.Count; i++)
                {
                    if (cachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                        }

                        cachedRoomList = newList;
                    }
                }
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/2";

            roomItem.GetComponent<RoomItemButtonHandler>().roomName = room.Name;
        }
    }

    public void JoinRoomByName(string name)
    {
        createAndJoinRooms.JoinRoom(name);
        
    }

}
