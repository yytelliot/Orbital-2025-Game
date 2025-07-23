using UnityEngine;

public class DebugOnGUIButton : MonoBehaviour
{
    public Rect buttonRect = new Rect(10, 10, 100, 30);
    public GameEvent gameEvent;

    void OnGUI()
    {
        if (GUI.Button(buttonRect, "RAISE EVENT!"))
        {
            DoDebugCallback();
        }
    }

    private void DoDebugCallback()
    {
        Debug.Log("Event button pressed!");
        gameEvent.Raise(this, null);
    }
}