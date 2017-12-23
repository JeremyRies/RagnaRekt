using System;
using System.Linq;
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
        private int[] _selectedCharacterIds;

        private int _counter = 0;

        void Start()
        {
            _counter = 0;
        }

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
            _selectedCharacterIds = null;
        }

        public void SetNumberPlayers(int i)
        {
            CharacterPreview.ResetAll();
            _numberPlayers = i;
            _selectedCharacterIds = new int[i];
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
            return _selectedCharacterIds;
        }

        public int GetCounter()
        {
            return _counter;
        }

        public void Select(int playerId, int characterId)
        {
            if (playerId >= 0 && playerId < _selectedCharacterIds.Length)
            {
                _selectedCharacterIds[playerId] = characterId;
                CharacterPreview.SetCharacter(playerId, characterId);
                CharacterPreview.EnablePreview(playerId, true);

                CheckAllCharactersSelected();
            }
            
        }

        private void CheckAllCharactersSelected()
        {
            if (_selectedCharacterIds.Any(selectedCharacterId => selectedCharacterId == 0))
                return;

            BackgroundMusic.BackgroundMusicInstance.StopPlay();

            Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => LevelController.Instance.LoadGameScene());


            var crossLevelDataTransfer = FindObjectOfType<CrossLevelDataTransfer>();

            if (crossLevelDataTransfer != null)
            {
                crossLevelDataTransfer.SaveSelectedCharacters(_selectedCharacterIds);
                crossLevelDataTransfer.SaveNumberPlayers(_numberPlayers);
            }
        }

        public void Select(int characterId)
        {
            Select(_counter, characterId);
            _counter++;
            UpdateInput();

        }

        public void RevertSelect(int playerId)
        {
            if (playerId > 0 && playerId < _selectedCharacterIds.Length)
            {
                _selectedCharacterIds[playerId] = 0;
                CharacterPreview.EnablePreview(playerId, false);
            }
        }

        public void RevertSelect()
        {
            Reset();

            foreach (var charButton in FindObjectsOfType<CharacterButton>())
            {
                charButton.ResetPlayerId();
            }

            Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_=> MenuHandler.SwitchToMainPanel());

        }
    }
}
