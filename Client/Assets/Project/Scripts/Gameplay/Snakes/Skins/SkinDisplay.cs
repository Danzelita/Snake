using Project.Scripts.Settings.Skins;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Skins
{
    public class SkinDisplay : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;

        public void SetSkin(SkinSettings skinSettings)
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers) 
                meshRenderer.material = skinSettings.Material;
        }
    }
}