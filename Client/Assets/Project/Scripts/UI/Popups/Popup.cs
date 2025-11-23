using System.Collections;
using UnityEngine;

namespace Project.Scripts.UI.Popups
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private const string OpenTrigger = "Open";
        private const string CloseTrigger = "Close";
        public void Open()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger(OpenTrigger);
            
            OnOpen();
        }

        public void Close(float delayClose = 0f)
        {
            StopAllCoroutines();
            StartCoroutine(CloseProcess(delayClose));
        }

        protected virtual void OnOpen() {}

        private IEnumerator CloseProcess(float delayClose = 0f)
        {
            yield return new WaitForSeconds(delayClose);
            _animator.SetTrigger(CloseTrigger);
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }
    }
}