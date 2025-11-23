using UnityEngine;

public class ScaleButtonAudioManager : MonoBehaviour
{
	public static ScaleButtonAudioManager Instance { get; private set; }
	[SerializeField] private AudioSource _source;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void PlayOneShot(AudioClip clip, float volume)
	{
		_source.PlayOneShot(clip, volume);
	}
}