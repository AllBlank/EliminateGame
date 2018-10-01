using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	private AudioSource aud;

	void Awake()
	{
		instance = this;
		aud = GetComponent<AudioSource> ();
	}

	public void PlayMagicalAudio()
	{
		StartCoroutine (PlayAudio());
	}

	IEnumerator PlayAudio()
	{
		yield return new WaitForSeconds (0.6f);
		if (!aud.isPlaying || aud.time > 0.1f)
			aud.Play ();
	}
}