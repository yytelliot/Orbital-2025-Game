using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LogConsole : MonoBehaviour
{
    public void ConsoleMessage(Component sender, object data)
    {
        Debug.developerConsoleVisible = true;
        if (data is int)
        {
            int amount = (int)data;
            string caller = sender.ToString();
            Debug.Log($"Recieved {amount} from {caller}");
        }
        else
        {
            Debug.Log("test");
        }
    }
}
