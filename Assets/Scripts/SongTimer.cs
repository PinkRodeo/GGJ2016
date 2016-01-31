using UnityEngine;

public class SongTimer
{
	public static bool isSongRunning = false;
	
	public static float initialTime;
	public static float bpm;
	public static float frequency;


	public static float delay;

	public static void StartSong(float newBPM, float delay = 0.0f)
	{
		SongTimer.delay = delay;

		bpm = newBPM;
		frequency = newBPM / 60f;

		initialTime = Time.time;

		isSongRunning = true;
	}

	public static void StopSong()
	{
		isSongRunning = false;

		bpm = 0f;
		frequency = 0f;

		initialTime = 0f;
	}

	/// <summary>
	/// Get a value that'll be timed to the current beat.
	/// Should be divided by a factor of 2, then run through Cos or Sin.
	/// </summary>
	/// <returns></returns>
	public static float timedValue(float multiplier = 1f)
	{
		if (isSongRunning)
		{
			return (2f*Mathf.PI*frequency/ multiplier * (Time.time - initialTime+ delay));
		}
		else
		{
			Debug.LogError("[SongTimer] something tried to timedValue() without a song being started");
			return 0f;
		}
	}

	private const float LEADIN_DURATION = 6f;

	public static float leadInRatio(float multiplier = 1f)
	{
		if (Time.time - initialTime < LEADIN_DURATION)
		{
			return (Time.time - initialTime) /LEADIN_DURATION;
		}
		else
		{
			return 1f;
		}
		if (isSongRunning)
		{
			return (2f * Mathf.PI * frequency / multiplier * (Time.time - initialTime));
		}
		else
		{
			Debug.LogError("[SongTimer] something tried to timedValue() without a song being started");
			return 0f;
		}
	}
}
