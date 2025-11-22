using UnityEngine;

namespace Project.Scripts.Settings.Foods
{
    [CreateAssetMenu(fileName = "FoodsSettings", menuName = "GameSettings/Foods/New Foods Settings")]
    public class FoodsSettings : ScriptableObject
    {
        public FoodSettings[] Foods;
    }
}