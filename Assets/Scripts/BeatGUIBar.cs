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
	public int timesTheAmountForSmallerChecks = 3;
	public Sprite[] sprites;

	private float timer;
	private float globalTime;

	private int currentIndex = 0;
	private int msCurrentIndex = 0;
	private int sBeatLength = 0;
	private int msBeatLength = 0;

	private Color barColor;
	private GameObject canvas;
	private AudioSource source;

	private Beat[] sBeatList;
	private AccurateBeat[] msBeatList;

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

	public struct AccurateBeat
	{
		public float time;
		public Beat mainBeat;
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

		while( msCurrentIndex < msBeatLength )
		{
			if ( msBeatList[msCurrentIndex].time <= globalTime )
			{
				if ( msBeatList[msCurrentIndex].time >= sBeatList[currentIndex].time )
				{
					if (Input.GetKeyDown(KeyCode.E))
					{
						Log.Tinas("Hit: " + DateTime.Now.ToString("mm:ss:fff") );
					}	
				}
				msCurrentIndex++;
			}
			else
			{
				break;
			}
		}

		while( currentIndex < sBeatLength )
		{
			if (sBeatList[currentIndex].time <= globalTime)
			{
				//we hit the beat in this frame so do some stuff
				Log.Tinas("Should get input now: " + DateTime.Now.ToString("mm:ss:fff") );
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
		sBeatLength = (int)( seconds * beatsPerSecond  );
		msBeatLength = sBeatLength * timesTheAmountForSmallerChecks;
		sBeatList = new Beat[ sBeatLength ];
		msBeatList = new AccurateBeat[ msBeatLength ];
		int amount = sBeatLength - (sBeatLength / 8);
		for( int i = 0; i < sBeatLength; i++ )
		{
			if ( i >= amount)
			{
				Beat b = sBeatList[ i ];
				b.type = BarType.empty;
				b.sprite = null;
				sBeatList[ i ] = b;
			} 
			Beat c = sBeatList[ i ];
			c.type = BarType.normal;
			c.time = DelayForMusic + i * timer;
			c.sprite = sprites[0];
			sBeatList[ i ] = c;
		}

		for( int i = 6 + startingAfter; i < sBeatLength; i += 8 )
		{
			Beat b = sBeatList[ i ];
			b.type = BarType.special;
			b.sprite = sprites[1];
			sBeatList[ i ] = b;

			Beat c = sBeatList[ i + 1 ];
			c.type = BarType.empty;
			sBeatList[ i + 1  ] = c;
		}
		float newDelay = DelayForMusic - timer * timesTheAmountForSmallerChecks;
		for( int i = 0; i < msBeatLength; i++ )
		{
			AccurateBeat b = msBeatList[ i ];
			b.time = DelayForMusic + i * ( timer / timesTheAmountForSmallerChecks );
			msBeatList[ i ] = b;
		}

		for( int i = 0; i < sBeatLength; i++ )
		{
			GameObject bar = new GameObject( "beatBar", typeof( RectTransform ) );
			bar.AddComponent<CanvasRenderer>();
			bar.AddComponent<Image>();

			RectTransform rt = bar.GetComponent<RectTransform>();
			bar.transform.SetParent( canvas.transform );
			Vector3 p = rt.position;

			rt.transform.position = new Vector3( p.x + i * 300.0f, p.y + 50.0f, p.z );

			BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
			behaviour.barSpeed = barSpeed;
			Beat a = sBeatList[ i ];
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
