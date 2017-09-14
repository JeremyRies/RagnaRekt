using System;
using JetBrains.Annotations;
using Sound;
using UniRx;
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
            BackgroundMusic.BackgroundMusicInstance.SetClip(ClipIdentifier.BackgroundMenu);
            BackgroundMusic.BackgroundMusicInstance.StartPlay();
        }

        public void OnStartTwoPlayer()
        {
            CharacterPickingHandler.SetNumberPlayers(2);

            Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => SwitchToCharacterPanel());
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

            Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => SwitchToCharacterPanel());
        }

        public void OnQuit()
        {
            Debug.Break();
        }
    }
}
