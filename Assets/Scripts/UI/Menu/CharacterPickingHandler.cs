using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class CharacterPickingHandler : MonoBehaviour
    {
        public StandaloneInputModule InputProvider;

        private int _numberPlayers = 2;
        private int[] _selectedCharacterIDs;

        private int _counter = 0;

        public void Reset()
        {
            SetNumberPlayers(2);
        }

        public void SetNumberPlayers(int i)
        {
            _numberPlayers = i;
            _selectedCharacterIDs = new int[i];
            _counter = 0;
            UpdateInput();
        }

        private void UpdateInput()
        {

            InputProvider.verticalAxis = "Vertical " + (_counter + 1);
        }


        public int[] GetSelectedCharacters()
        {
            return _selectedCharacterIDs;
        }

        public void Select(int id)
        {
            if (_counter + 1 >= _numberPlayers) return;
            _selectedCharacterIDs[_counter] = id;
            _counter++;
            UpdateInput();
        }

        public void RevertSelect()
        {
            if (_counter > 1)
            {
                _selectedCharacterIDs[_counter] = -1;
                _counter--;
                UpdateInput();
            }
        }
    }
}
