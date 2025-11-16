using UnityEngine;

namespace Project.Scripts.Settings.Skins
{
    [CreateAssetMenu(fileName = "SkinSettings", menuName = "GameSettings/Skins/New Skin Settings")]
    public class SkinSettings : ScriptableObject
    {
        public Material Material;
    }
}