using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class BeatGUIBar : MonoBehaviour 
{
	public float barSpeed;
	public float offset;
	public int BeatsPerMinute;
	public float delay;
	public int startingAfter;
	public List<BarType> pattern;
	private int counter;
	private int changeOnEveryOther;
	private int patternIndex;
	private Color barColor;
	private GameObject canvas;
	private AudioSource source;

	public BarType[] beatList;

	public enum BarType
	{
		normal,
		special,
		empty
	}

	BarType type;

	void Start () 
	{
		
		type = BarType.normal;
		canvas = GameObject.Find("Canvas");
		//Invoke( "AddBeatBar", 60.0f / BeatsPerMinute );
		source = GetComponent<AudioSource>();
		//GenerateArray( source.clip.length );
		if( source != null )
		{
			source.PlayDelayed( delay );
		}
		changeOnEveryOther = startingAfter;
		patternIndex = -1;
		barColor = Color.white;
	}

	void Update () 
	{
		

	}

	void GenerateArray( float seconds )
	{
		float beatsPerSecond = 60.0f / BeatsPerMinute;
		int size = (int)( seconds / beatsPerSecond  );
		beatList = new BarType[ size ];
		int amount = size - (size / 6);
		for( int i = 0; i < size; i++ )
		{
			if ( i >= amount)
			{
				beatList[ i ] = BarType.empty;
			}
			if( Convert.ToBoolean(i & 3) )
			{
				beatList[ i ] = BarType.special;
				beatList[ i + 1 ] = BarType.empty;
			}
			else
			{
				beatList[ i ] = BarType.normal;
			}
		}


		for( int i = 0; i < size; i++ )
		{
			GameObject bar = new GameObject( "beatBar", typeof( RectTransform ) );
			bar.AddComponent<CanvasRenderer>();
			bar.AddComponent<Image>();

			RectTransform rt = bar.GetComponent<RectTransform>();
			bar.transform.SetParent( canvas.transform );
			Vector3 p = rt.position;

			rt.transform.position = new Vector3( p.x + Screen.currentResolution.width + i * 100.0f, p.y, p.z );

			BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
			behaviour.barSpeed = barSpeed;
			BarType a = beatList[ i ];
			if( a == BarType.normal ) barColor = Color.blue;
			if( a == BarType.special ) barColor = Color.red;
			if( a == BarType.empty ) barColor = Color.yellow;
			bar.GetComponent<CanvasRenderer>().SetColor( barColor );
		}
	}

	int GetCount( BarType type )
	{
		int count = -1;
		switch( type )
		{
			case BarType.normal:
				count = 5;
				break;
			case BarType.special:
				count = 0;
				break;
			case BarType.empty:
				count = 0;
				break;
			default:
				break;
		}
		return count;
	}

	void GetPatternType( GameObject bar )
	{
		if ( pattern[ patternIndex ] == BarType.normal ) barColor = Color.blue;
		if ( pattern[ patternIndex ] == BarType.special ) barColor = Color.red;
		if ( pattern[ patternIndex ] == BarType.empty ) barColor = Color.yellow;
	}

	private void AddBeatBar()
	{
		GameObject bar = new GameObject( "beatBar", typeof(RectTransform) );
		bar.AddComponent<CanvasRenderer>();
		bar.AddComponent<Image>();

		RectTransform rt = bar.GetComponent<RectTransform>();
		bar.transform.SetParent( canvas.transform );
		Vector3 p = rt.position;

		rt.transform.position = new Vector3( p.x + Screen.currentResolution.width, p.y, p.z );

		BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
		behaviour.barSpeed = barSpeed;

		if( changeOnEveryOther == counter )
		{
			counter = 0;
			++patternIndex;
			if( patternIndex >= pattern.Count ) patternIndex = 0;
			changeOnEveryOther = GetCount(pattern[ patternIndex ]);
			GetPatternType(bar);
		}
		else
		{
			counter++;
		}
		bar.GetComponent<CanvasRenderer>().SetColor(barColor);

		Invoke( "AddBeatBar", 60.0f / BeatsPerMinute );
	}
}
