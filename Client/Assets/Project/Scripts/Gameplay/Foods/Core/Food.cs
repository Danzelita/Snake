using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Gameplay.Foods.Core
{
    public class Food : MonoBehaviour
    {
        private FoodState _foodState;
        private Action _onCollect;

        public void Init(FoodState foodState, Action onCollect)
        {
            _foodState = foodState;
            _foodState.OnChange += FoodStateOnOnChange;
            _onCollect = onCollect;
        }

        private void FoodStateOnOnChange(List<DataChange> changes)
        {
        }

        public void Destroy()
        {
            _foodState.OnChange -= FoodStateOnOnChange;
            Destroy(gameObject);
        }

        public void Collect()
        {
            _onCollect.Invoke();
            gameObject.SetActive(false);
        }
    }
}