using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Song 
{
	public AudioClip audioClip;
	public List<BeatAction> beatArray;
	public int beatsPerMinute;

	public Song()
	{
		Log.Tinas( "Create Song object" );
	}

}
 