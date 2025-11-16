using UnityEngine;

namespace Project.Scripts.Settings.Skins
{
    [CreateAssetMenu(fileName = "SkinsSettings", menuName = "GameSettings/Skins/New Skins Settings")]
    public class SkinsSettings : ScriptableObject
    {
        public SkinSettings[] Skins;
    }
}