﻿using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class MenuHandler : MonoBehaviour
    {
        public GameObject FirstGameObjectSelected;
        public GameObject MainMenuPanel;
        public GameObject CharacterPickingPanel;

        public CharacterPickingHandler CharacterPickingHandler;

        private void Start()
        {
            CharacterPickingHandler.Reset();
            SwitchToMainPanel();
        }

        public void OnStartTwoPlayer()
        {
            CharacterPickingHandler.SetNumberPlayers(2);

            SwitchToCharacterPanel();
        }

        public void SwitchToCharacterPanel()
        {
            MainMenuPanel.SetActive(false);
            CharacterPickingPanel.SetActive(true);
        }

        public void SwitchToMainPanel()
        {
            MainMenuPanel.SetActive(true);
            CharacterPickingPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(FirstGameObjectSelected);
            CharacterPickingHandler.Reset();
        }

        public void OnStartFourPlayer()
        {
            CharacterPickingHandler.SetNumberPlayers(4);

            SwitchToCharacterPanel();
        }

        public void OnQuit()
        {
#if DEBUG
            Debug.Break();
#else
        Application.Stop();
#endif
        }
    }
}