using Project.Scripts.UI.Popups;
using Project.Scripts.UI.Screens.Gameplay;
using UnityEngine;
using Screen = Project.Scripts.UI.Screens.Screen;

namespace Project.Scripts.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private StarterPopup _starterPopup;
        [SerializeField] private GameplayScreen _gameplayScreen;
        [SerializeField] private Transform _loadingScreen;
        
        private Screen _currentScreen;
        
        public void ShowLoadingScreen() => 
            _loadingScreen.gameObject.SetActive(true);

        public void HideLoadingScreen() => 
            _loadingScreen.gameObject.SetActive(false);

        public void OpenStarterPopup() => 
            _starterPopup.Open();

        public void CloseStarterPopup() => 
            _starterPopup.Close();

        public void OpenGameplayScreen()
        {
            if (_currentScreen == _gameplayScreen)
                return;
            
            _gameplayScreen.Open();
        }
    }
}