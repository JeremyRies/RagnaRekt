using NDream.AirConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConsoleViewManager : MonoBehaviour {

    public const ViewID STARTING_VIEW = ViewID.MainMenu;

    private void Start()
    {
        DontDestroyOnLoad(this);
        AirConsole.instance.onReady += OnReady;
    }

    void OnReady(string code)
    {
        ShowView(STARTING_VIEW);
    }

    public static void ShowView(ViewID viewId)
    {
        switch(viewId)
        {
            case ViewID.CharacterSelection:
                Debug.Log("View Changed");
                AirConsole.instance.SetCustomDeviceStateProperty("ctrl_view", "CharacterSelection");
                break;
            case ViewID.MainMenu:
                Debug.Log("View Changed");
                AirConsole.instance.SetCustomDeviceStateProperty("ctrl_view", "MainMenu");
                break;
        }

    }

    public enum ViewID{
        MainMenu,
        CharacterSelection,
        InGameController

    }
}
