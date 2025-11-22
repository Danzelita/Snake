using UnityEngine;

namespace Project.Scripts.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private Transform _loadingScreen;
        
        public void ShowLoadingScreen() => 
            _loadingScreen.gameObject.SetActive(true);

        public void HideLoadingScreen() => 
            _loadingScreen.gameObject.SetActive(false);

        public void OpenStarterPopup()
        {
            
        }

        public void CloseStarterPopup()
        {
            
        }
    }
}