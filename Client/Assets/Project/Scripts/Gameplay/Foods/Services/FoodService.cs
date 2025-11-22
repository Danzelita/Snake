using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Data;
using Project.Scripts.Gameplay.Foods.Core;
using Project.Scripts.Multiplayer;
using Project.Scripts.Multiplayer.Generated;
using Project.Scripts.Settings.Foods;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts.Gameplay.Foods.Services
{
    public class FoodService : IDisposable
    {
        private MapSchema<FoodState> _foodState;
        private Dictionary<FoodType, FoodSettings> _foodsSettings = new();
        private Dictionary<string, object> _data = new();
        
        private readonly MultiplayerManager _multiplayerManager;
        private readonly Dictionary<string, Food> _foods = new();
        
        private const string Id = "i";
        private const string Type = "t";
        private const string Score = "s";
        
        public FoodService(MultiplayerManager multiplayerManager, FoodsSettings foodsSettings)
        {
            _multiplayerManager = multiplayerManager;

            foreach (FoodSettings foodSettings in foodsSettings.Foods) 
                _foodsSettings.Add(foodSettings.Type, foodSettings);
        }

        public void Init(MapSchema<FoodState> foodsStates)
        {
            _foodState = foodsStates;
            
            _foodState.ForEach(CreateFood);
            _foodState.OnAdd += CreateFood;
            _foodState.OnRemove += RemoveFood;
        }

        private void CreateFood(string key, FoodState foodState)
        {
            Vector3 position = foodState.position.ToVector3();
            FoodSettings foodSettings = _foodsSettings[Enum.Parse<FoodType>(foodState.type, ignoreCase: true)];
            Food newFood = Object.Instantiate(foodSettings.Prefab, position, Quaternion.identity);
            newFood.Init(foodState, onCollect: () =>
            {
                _data[Id] = key;
                _data[Type] = foodSettings.Type.ToString();
                _data[Score] = foodSettings.Score;
                _multiplayerManager.SendToServer("collect", _data);

                string a = $"{key}|{(int)foodSettings.Type}|{foodSettings.Score}";
                Debug.Log(a);
            });
            
            _foods.Add(key, newFood);
        }

        private void RemoveFood(string key, FoodState foodState)
        {
            if (_foods.Remove(key, out Food food) == false)
                return;

            food.Destroy();
        }

        public void Dispose()
        {
            foreach (Food food in _foods.Values) 
                food.Destroy();
            
            _foods.Clear();
        }
    }
}