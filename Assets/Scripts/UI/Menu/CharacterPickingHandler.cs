using System;
using Control;
using Sound;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class CharacterPickingHandler : MonoBehaviour
    {
        public IInputProvider UnityInputProvider; 
        public StandaloneInputModule StandaloneInputProvider;
        public GameObject FirstCharacter;

        public CharacterPreview CharacterPreview;
        public MenuHandler MenuHandler;

        private int _numberPlayers = 2;
        private int[] _selectedCharacterIDs;

        private int _counter = 0;

        void Update()
        {
            if (UnityInputProvider.GetButtonDown("Attack"))
            {
                RevertSelect();
            }
        }

        public void Reset()
        {
            CharacterPreview.ResetAll();
            _counter = 0;
            UnityInputProvider = new UnityInputProvider(1);
            UpdateInput();
            _selectedCharacterIDs = null;
        }

        public void SetNumberPlayers(int i)
        {
            _numberPlayers = i;
            _selectedCharacterIDs = new int[i];
            _counter = 0;
            EventSystem.current.SetSelectedGameObject(FirstCharacter);
            UpdateInput();
        }

        private void UpdateInput()
        {
            StandaloneInputProvider.horizontalAxis = "Horizontal " + (_counter + 1);
            StandaloneInputProvider.verticalAxis = "Vertical " + (_counter + 1);
            StandaloneInputProvider.submitButton = "Jump " + (_counter + 1);
        }


        public int[] GetSelectedCharacters()
        {
            return _selectedCharacterIDs;
        }

        public int GetCounter()
        {
            return _counter;
        }

        public void Select(int id)
        {
            if (_counter + 1 < _numberPlayers)
            {
                _selectedCharacterIDs[_counter] = id;
                CharacterPreview.SetCharacter(_counter, id);
                CharacterPreview.EnablePreview(_counter, true);
                _counter++;
                UpdateInput();
            }
            else if (_counter + 1 == _numberPlayers)
            {
                _selectedCharacterIDs[_counter] = id;
                CharacterPreview.SetCharacter(_counter, id);
                CharacterPreview.EnablePreview(_counter, true);

                BackgroundMusic.BackgroundMusicInstance.StopPlay();

                Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_=> LevelController.GetInstance().LoadGameScene());


                var crossLevelDataTransfer = FindObjectOfType<CrossLevelDataTransfer>();
                if (crossLevelDataTransfer != null)
                {
                    crossLevelDataTransfer.SaveSelectedCharacters(_selectedCharacterIDs);
                    crossLevelDataTransfer.SaveNumberPlayers(_numberPlayers);
                }

           
            }

        }

        public void RevertSelect()
        {
            Reset();
            Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_=> MenuHandler.SwitchToMainPanel());

        }
    }
}
