using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;
using UniRx;
using System;

public class AirConsoleMenuHandler : MonoBehaviour
{
    public CharacterPickingHandler CharacterPickingHandler;
    public MenuHandler MenuHandler;

	void Awake ()
	{
	    AirConsole.instance.onMessage += OnMessage;
	}

    void OnMessage(int deviceId, JToken data)
    {
        var playerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);
        if (playerNumber == 0)
        {        
            HandleMainMenuActions(data);
        }
        HandleCharacterPickingActions(playerNumber, data);
    }

    private void HandleCharacterPickingActions(int playerNumber, JToken data)
    {
        var characterData = (string)data["Character"];

        if (characterData == null)
            return;

        var selectedCharacter = CharacterIds.CharacterIdentifier[characterData];

        CharacterPickingHandler.Select(playerNumber, selectedCharacter);
    }

    private void HandleMainMenuActions(JToken data)
    {
        var menuData = (string)data["MainMenu"];

        if (menuData == null)
            return;

        if (menuData.Equals("2player"))
        {
                AirConsoleViewManager.ShowView(AirConsoleViewManager.ViewID.CharacterSelection);

                MenuHandler.OnStartTwoPlayer();
        }
        else if (menuData.Equals("4player"))
        {
            AirConsoleViewManager.ShowView(AirConsoleViewManager.ViewID.CharacterSelection);

            MenuHandler.OnStartFourPlayer();
        }

    }
}
