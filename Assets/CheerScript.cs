using UnityEngine;
using System.Collections;

public class CheerScript : MonoBehaviour {

	public AudioClip[] cheers;
	public AudioClip endCheer;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayCheer () {
		if(audioSource.isPlaying) {
			return;
		}
		int idx = Random.Range(0, cheers.Length -1);
		audioSource.clip = cheers [idx];
		audioSource.Play ();
	}

	public void StartEndCheer () {
		audioSource.clip = endCheer;
		audioSource.loop = true;
		audioSource.Play ();
	}

	public void StopEndCheer () {
		audioSource.loop = false;
		audioSource.Stop ();
	}
}
