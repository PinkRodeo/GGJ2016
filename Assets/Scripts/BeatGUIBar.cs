using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Timers;

public class BeatGUIBar : MonoBehaviour 
{
	public float barSpeed = 300.0f;
	public int BeatsPerMinute = 128;
	public float DelayForMusic = 3.9f;
	public int startingAfter = 15;

	private float timer;
	private float globalTime;

	private int spawnIndex = 0;
	private int currentIndex = 0;
	private int beatLength = 0;

	private Color barColor;
	private GameObject canvas;
	private AudioSource source;

	public Beat[] beatList;

	public enum BarType
	{
		normal,
		special,
		empty
	}

	public struct Beat
	{
		public float time;
		public BarType type;
	};

	void Start () 
	{
		timer = 60.0f / BeatsPerMinute;
		canvas = GameObject.Find("Canvas");
		source = GetComponent<AudioSource>();
		GenerateArray( source.clip.length );
		if( source != null )
		{
			source.PlayDelayed( DelayForMusic );
		}
	}

	void Update () 
	{
		globalTime += Time.deltaTime;

		while( currentIndex < beatLength )
		{
			if (beatList[currentIndex].time <= globalTime)
			{
				//we hit the beat in this frame so do some stuff
				Log.Tinas("BEAT HITSZ@!" + currentIndex);
				currentIndex++;
			}
			else 
			{
				break;
			}
		}
		/*
		while(spawnIndex < beatLength )
		{
			if (beatList[spawnIndex].time <= globalTime + DelayForMusic)
			{
				//This beat will be hit in 'delay' seconds so we spawn it now
				spawnIndex++;
			} 
			else 
			{
				break;
			}
		}
		*/

	}
		

	void GenerateArray( float seconds )
	{
		float beatsPerSecond = 1.0f / timer;
		beatLength = (int)( seconds * beatsPerSecond  );
		beatList = new Beat[ beatLength ];
		int amount = beatLength - (beatLength / 8);
		for( int i = 0; i < beatLength; i++ )
		{
			if ( i >= amount)
			{
				Beat b = beatList[ i ];
				b.type = BarType.empty;
				beatList[ i ] = b;
			} 
			Beat c = beatList[ i ];
			c.type = BarType.normal;
			c.time = DelayForMusic + i * timer;
			beatList[ i ] = c;
		}

		for( int i = 6 + startingAfter; i < beatLength; i += 8 )
		{
			Beat b = beatList[ i ];
			b.type = BarType.special;
			beatList[ i ] = b;

			Beat c = beatList[ i + 1 ];
			c.type = BarType.empty;
			beatList[ i + 1  ] = c;
		}


		for( int i = 0; i < beatLength; i++ )
		{
			GameObject bar = new GameObject( "beatBar", typeof( RectTransform ) );
			bar.AddComponent<CanvasRenderer>();
			bar.AddComponent<Image>();

			RectTransform rt = bar.GetComponent<RectTransform>();
			bar.transform.SetParent( canvas.transform );
			Vector3 p = rt.position;

			rt.transform.position = new Vector3( p.x + Screen.currentResolution.width + i * 300.0f, p.y, p.z );

			BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
			behaviour.barSpeed = barSpeed;
			Beat a = beatList[ i ];
			if( a.type == BarType.normal ) barColor = Color.blue;
			if( a.type == BarType.special ) barColor = Color.red;
			if( a.type == BarType.empty ) barColor = Color.yellow;
			bar.GetComponent<CanvasRenderer>().SetColor( barColor );
		}
	}
}
