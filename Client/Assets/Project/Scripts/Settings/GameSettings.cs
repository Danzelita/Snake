using Project.Scripts.Settings.Foods;
using Project.Scripts.Settings.Skins;
using UnityEngine;

namespace Project.Scripts.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public SkinsSettings SkinsSettings;
        public FoodsSettings FoodsSettings;
    }
}