using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EventManager : MonoBehaviour
{
    public void ConsoleMessage(Component sender, object data)
    {
        if (data is int)
        {
            int amount = (int)data;
            string caller = sender.ToString();
            Debug.Log($"Recieved {amount} from {caller}");
            
            if (PhotonNetwork.LocalPlayer.CustomProperties["PlayerRole"].ToString() == "Technician")
            {
                Debug.Log("hit");
                //CrossSceneNetworkManager.Instance.SendNetworkEvent(sender, data);
                
                // Local technician effects
                //PlayLocalEffects();
            }
        }
        else
        {
            Debug.Log("test");
        }
    }
}
