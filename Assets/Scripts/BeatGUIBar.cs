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
	public Sprite[] sprites;

	private float timer;
	private float globalTime;

	private int spawnIndex = 0;
	private int currentIndex = 0;
	private int beatLength = 0;

	private Color barColor;
	private GameObject canvas;
	private AudioSource source;

	private Beat[] beatList;

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
		public Sprite sprite;
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
				b.sprite = null;
				beatList[ i ] = b;
			} 
			Beat c = beatList[ i ];
			c.type = BarType.normal;
			c.time = DelayForMusic + i * timer;
			c.sprite = sprites[0];
			beatList[ i ] = c;
		}

		for( int i = 6 + startingAfter; i < beatLength; i += 8 )
		{
			Beat b = beatList[ i ];
			b.type = BarType.special;
			b.sprite = sprites[1];
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

			rt.transform.position = new Vector3( p.x + Screen.currentResolution.width + i * 300.0f, p.y + 50.0f, p.z );

			BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
			behaviour.barSpeed = barSpeed;
			Beat a = beatList[ i ];
			bar.GetComponent<Image>().sprite = a.sprite;
			bar.GetComponent<Image>().SetNativeSize(); 
			if (a.type == BarType.special)
			{
				GameObject barSpecial = new GameObject( "beatBar", typeof( RectTransform ) );
				barSpecial.AddComponent<CanvasRenderer>();
				barSpecial.AddComponent<Image>();

				RectTransform rtSpecial = barSpecial.GetComponent<RectTransform>();
				barSpecial.transform.SetParent( canvas.transform );
				Vector3 pSpecial = rtSpecial.position;

				rtSpecial.transform.position = new Vector3( pSpecial.x + Screen.currentResolution.width + i * 300.0f, pSpecial.y + 120.0f, pSpecial.z );

				BeatBarBehaviour behaviour2 = barSpecial.AddComponent<BeatBarBehaviour>();
				behaviour2.barSpeed = barSpeed;
				barSpecial.GetComponent<Image>().sprite = sprites[2];
				barSpecial.GetComponent<Image>().SetNativeSize(); 
			}
		}
	}
}
