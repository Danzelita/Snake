using Project.Scripts.Gameplay.Foods.Core;
using UnityEngine;

namespace Project.Scripts.Settings.Foods
{
    [CreateAssetMenu(fileName = "FoodSettings", menuName = "GameSettings/Foods/New Food Settings")]
    public class FoodSettings : ScriptableObject
    {
        public FoodType Type = FoodType.None;
        public ushort Score;
        public Food Prefab;
    }
}