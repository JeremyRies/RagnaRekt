using UnityEngine;

namespace UI.Menu
{
    public class MenuHandler : MonoBehaviour
    {
        public GameObject MainMenuPanel;
        public GameObject CharacterPickingPanel;

        public CharacterPickingHandler CharacterPickingHandler;

        public void OnStartTwoPlayer()
        {
            CharacterPickingHandler.SetNumberPlayers(2);

            SwitchToCharacterPanel();
        }

        private void SwitchToCharacterPanel()
        {
            MainMenuPanel.SetActive(false);
            CharacterPickingPanel.SetActive(true);
        }

        private void SwitchToMainPanel()
        {
            MainMenuPanel.SetActive(true);
            CharacterPickingPanel.SetActive(false);
        }

        public void OnStartFourPlayer()
        {
            CharacterPickingHandler.SetNumberPlayers(4);

            SwitchToCharacterPanel();
        }

        private void SwitchToCharacterScreen()
        {
        
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
