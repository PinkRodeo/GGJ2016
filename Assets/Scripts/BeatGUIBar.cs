using UnityEngine;
using UnityEngine.UI;
using System;

public class BeatGUIBar : MonoBehaviour
{
	public int BeatsPerMinute = 128;
	public float DelayForMusic = 0.0f;
	public float DelayForBeats = 0f;
	public int startingAfter = 15;
	public int timesTheAmountForSmallerChecks = 1;
	public Sprite[] sprites;
	public Sprite[] poseSprites;


	private float timeBetweenBeats;
	public float globalTime;
	private float? _rhythmStartTime = null;

	private bool songStarted;
	private int currentIndex = 0;
	private int msCurrentIndex = 0;
	private int sBeatLength = 0;
	private int msBeatLength = 0;

	private Color barColor;
	private GameObject canvas;
	private AudioSource source;

	private Beat[] sBeatList;
	private AccurateBeat[] msBeatList;
	public TextAsset poses;
	private GameSceneMaster gameManager;

	public enum BarType
	{
		Normal,
		Special
	}

	public struct Beat
	{
		public float time;
		public BarType type;
		public Sprite sprite;
		public Pose pose;
		public int count;
	};

	public struct AccurateBeat
	{
		public float time;
		public Beat mainBeat;
	};

	private void Start()
	{
		Globals.Init( poses );
		gameManager = GameObject.Find("GameManager").GetComponent<GameSceneMaster>();
	}

	public void StartTheMusic()
	{
		timeBetweenBeats = 60.0f / BeatsPerMinute;
		canvas = GameObject.Find("Canvas");
		source = GetComponent<AudioSource>();
		GenerateArray( source.clip.length );
		if( source != null )
		{
			//source.PlayDelayed( DelayForMusic );


			Invoke("delayedSongStart", DelayForMusic);
			//Invoke("delayedBeatStart", DelayForBeats);

		}
	}


	// Called with Invoke() in Start()
	// ReSharper disable once UnusedMember.Local
	private  void delayedSongStart()
	{
		source.Play();
		songStarted = true;

		SongTimer.sourceToSampleTimeFrom = source;
		SongTimer.StartSong(128f, DelayForBeats);

		_rhythmStartTime = source.time;
	}

	// ReSharper disable once UnusedMember.Local
	private void delayedBeatStart()
	{
	}

	public bool IsSongFinished()
	{
		if (!songStarted) return false;
		return !source.isPlaying;
	}

	void Update ()
	{
		if (IsSongFinished())
		{
			gameManager.End();
		}

		if (_rhythmStartTime.HasValue)
		{
			globalTime = source.time - _rhythmStartTime.Value  + DelayForBeats;
		}


		while ( msCurrentIndex < msBeatLength )
		{
			//current index time lesser than global time
			if ( msBeatList[msCurrentIndex].time <= globalTime )
			{
				//current greater or equal to big (visual) beat time
				if ( msBeatList[msCurrentIndex].time >= sBeatList[currentIndex].time )
				{
					if (sBeatList[msCurrentIndex].type == BarType.Special)
					{
						gameManager.HitFullBeat(sBeatList[msCurrentIndex].pose);
					}
					else
					{
						gameManager.HitSubBeat();
					}
				}
				msCurrentIndex++;
			}
			else
			{
				break;
			}
		}
	}

	private static readonly Vector2 RegularBeatPivot = new Vector2(0.5f, 0);

	private void GenerateArray( float seconds )
	{
		Initialization(seconds);
		InitializeVisualBeatList();
		InitializeSpecialVisualBeats();

		SetBeatTimes();

		//TODO:
//		for( int i = 0; i < sBeatLength; i++ )
		for( int i = 0; i < sBeatLength; i++ )
		{

			GameObject bar = new GameObject( "beatBar", typeof( RectTransform ) );
			bar.AddComponent<CanvasRenderer>();
			bar.AddComponent<Image>();

			RectTransform rt = bar.GetComponent<RectTransform>();
			bar.transform.SetParent( canvas.transform );
			Vector3 p = rt.position;
			int spawnOffSetX = 1500;
			rt.anchorMax = Vector2.zero;
			rt.anchorMin = Vector2.zero;
			rt.pivot = RegularBeatPivot;

			rt.transform.position = new Vector3( p.x + spawnOffSetX + i * 300.0f, 0f, p.z );


			Beat beat = sBeatList[i];

			BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
			behaviour.beatController = this;
			behaviour.beat = beat;

			behaviour.Initialize();

			bar.GetComponent<Image>().sprite = beat.sprite;
			bar.GetComponent<Image>().SetNativeSize();
			if (beat.type == BarType.Special)
			{
				SpawnSpecialBeat(spawnOffSetX, i, beat);
			}
		}
	}

	private void SetBeatTimes()
	{
		for (int i = 0; i < msBeatLength; i++)
		{
			AccurateBeat beat = msBeatList[i];
			beat.time = i*(timeBetweenBeats/timesTheAmountForSmallerChecks);
			msBeatList[i] = beat;
		}
	}

	private void SpawnSpecialBeat(int spawnOffSetX, int index, Beat beat)
	{
		GameObject barSpecial = new GameObject("beatBar", typeof (RectTransform));
		barSpecial.AddComponent<CanvasRenderer>();
		barSpecial.AddComponent<Image>();

		RectTransform rtSpecial = barSpecial.GetComponent<RectTransform>();
		barSpecial.transform.SetParent(canvas.transform);
		Vector3 pSpecial = rtSpecial.position;

		rtSpecial.transform.position = new Vector3(pSpecial.x + spawnOffSetX + index*300.0f-100.0f, pSpecial.y + 120.0f, pSpecial.z);

		BeatBarBehaviour behaviour2 = barSpecial.AddComponent<BeatBarBehaviour>();

		behaviour2.beatController = this;
		behaviour2.beat = beat;

		behaviour2.Initialize();


		barSpecial.GetComponent<Image>().sprite = sBeatList[ index ].pose.data.uiTexture;
		barSpecial.GetComponent<Image>().SetNativeSize();
	}

	private void InitializeSpecialVisualBeats()
	{
		int specialBeatInterval = 4;
		for (int i = 6 + startingAfter; i < sBeatLength; i += specialBeatInterval)
		{
			Beat b = sBeatList[i];
			b.type = BarType.Special;
			int poseIndex = UnityEngine.Random.Range( 0, Globals.poses.Length );
			b.pose = new Pose( Globals.poses[ poseIndex ] );
			b.sprite = sprites[1];
			sBeatList[i] = b;
		}
	}

	private void Initialization(float seconds)
	{
		float beatsPerSecond = ((float)BeatsPerMinute)/60.0f;
		sBeatLength = (int) (seconds*beatsPerSecond);
		msBeatLength = sBeatLength*timesTheAmountForSmallerChecks;
		sBeatList = new Beat[sBeatLength];
		msBeatList = new AccurateBeat[msBeatLength];
	}

	private void InitializeVisualBeatList()
	{
		for (int i = 0; i < sBeatLength; i++)
		{
			Beat beat = sBeatList[i];
			beat.type = BarType.Normal;
			beat.time = i*timeBetweenBeats;
			beat.sprite = sprites[0];
			beat.count = i%8;
			sBeatList[i] = beat;
		}
	}
}
