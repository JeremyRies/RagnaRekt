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
        Debug.Log("Player number: " + playerNumber);
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
        var mainMenuData = (string) data["element"];
        Debug.Log("mainMenuData " + mainMenuData);

        for (int i = 1; i < 3; i++)
        {
            var buttonString = "MainMenu-" + i;      

            if (mainMenuData != buttonString) return;

            var menuData = (string)data["data"]["MainMenu"];

            Debug.Log("Menu Data: "+ menuData);

            if (menuData.Equals("2player"))
                MenuHandler.OnStartTwoPlayer();
            else if (menuData.Equals("4player"))
                MenuHandler.OnStartFourPlayer();
        }

    }
}
