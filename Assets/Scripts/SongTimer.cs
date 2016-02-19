using UnityEngine;

public class SongTimer
{
	public static bool isSongRunning;

	public static float initialTime;
	public static float bpm;
	public static float frequency;

	public static AudioSource sourceToSampleTimeFrom;

	//public static float delay;

	public static void StartSong(float newBPM, float delay = 0.0f)
	{
		//SongTimer.delay = delay;

		bpm = newBPM;
		frequency = newBPM / 60f;

		initialTime = GetCurrentTime() + delay + frequency/4f;

		isSongRunning = true;
	}

	public static void StopSong()
	{
		isSongRunning = false;

		bpm = 0f;
		frequency = 0f;

		initialTime = 0f;
	}

	private static float GetCurrentTime()
	{
		if (sourceToSampleTimeFrom != null)
		{
			return sourceToSampleTimeFrom.time;
		}
		else
			return Time.time;
	}

	/// <summary>
	/// Get a value that'll be timed to the current beat.
	/// Should be divided by a factor of 2, then run through Cos or Sin.
	/// </summary>
	/// <returns></returns>
	public static float TimedValue(float multiplier = 1f)
	{
		if (isSongRunning)
		{
			return (2f*Mathf.PI*frequency/ multiplier * (GetCurrentTime() - initialTime));
		}
		else
		{
			Debug.LogError("[SongTimer] something tried to timedValue() without a song being started");
			return 0f;
		}
	}

	private const float LEADIN_DURATION = 12f;

	public static float LeadInRatio(float multiplier = 1f)
	{
		if (GetCurrentTime() - initialTime < LEADIN_DURATION)
		{
			return (GetCurrentTime() - initialTime) /LEADIN_DURATION;
		}
		else
		{
			return 1f;
		}
		//if (isSongRunning)
		//{
		//	return (2f * Mathf.PI * frequency / multiplier * (GetCurrentTime() - initialTime));
		//}
		//else
		//{
		//	Debug.LogError("[SongTimer] something tried to timedValue() without a song being started");
		//	return 0f;
		//}
	}
}
