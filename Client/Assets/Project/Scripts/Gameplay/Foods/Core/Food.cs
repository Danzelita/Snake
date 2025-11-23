using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Gameplay.Foods.Core
{
    public class Food : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        
        public Action OnCollect;
        
        private FoodState _foodState;
        private Action _sendMessageOnCollect;


        public void Init(FoodState foodState, Action onCollect)
        {
            _foodState = foodState;
            _foodState.OnChange += FoodStateOnOnChange;
            _sendMessageOnCollect = onCollect;
        }

        private void FoodStateOnOnChange(List<DataChange> changes)
        {
        }

        public void Destroy()
        {
            _foodState.OnChange -= FoodStateOnOnChange;
            //StartCoroutine(DelayDestroy());
            Destroy(gameObject);
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        public void Collect()
        {
            _sendMessageOnCollect?.Invoke();
            OnCollect?.Invoke();
        }
    }
}