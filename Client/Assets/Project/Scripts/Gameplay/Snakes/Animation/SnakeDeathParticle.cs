using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Animation
{
    public class SnakeDeathParticle : MonoBehaviour
    {
        public void Init(Material material)
        {
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            renderer.material = material;
        }
    }
}