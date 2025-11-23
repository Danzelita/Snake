using UnityEngine;

[CreateAssetMenu]
public class ScaleButtonConfig : ScriptableObject
{
	public float PressedScale = 0.9f;
	public float Duration = 0.06f; // время анимации
	public AudioClip PressSound;
	public AudioClip ReleaseSound;
}
