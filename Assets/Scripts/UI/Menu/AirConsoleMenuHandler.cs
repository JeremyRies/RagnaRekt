using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

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
        Debug.Log(data);
        if (playerNumber == 0)
        {
            HandleMainMenuActions(data);
        }
        HandleCharacterPickingActions(playerNumber, data);
    }

    private void HandleCharacterPickingActions(int playerNumber, JToken data)
    {
        for (int i = 1; i < 3; i++)
        {
            var buttonString = "Character-" + i;
            var characterButtonData = data[buttonString];
            if (characterButtonData == null) return;

            var characterData = (string)characterButtonData["Character"];

            var selectedCharacter = CharacterIds.CharacterIdentifier[characterData];

            CharacterPickingHandler.Select(playerNumber, selectedCharacter);
        }
    }

    private void HandleMainMenuActions(JToken data)
    {
        for (int i = 1; i < 3; i++)
        {
            var buttonString = "MainMenu-" + i;

            var mainMenuData = data[buttonString];
            if (mainMenuData == null) return;

            var menuData = (string)mainMenuData["MainMenu"];

            if (menuData.Equals("2player"))
                MenuHandler.OnStartTwoPlayer();
            else if (menuData.Equals("4player"))
                MenuHandler.OnStartFourPlayer();
        }

    }
}
