using UnityEngine;
using System.Collections;

public class Frequency{
    private static Frequency instance;
    public float startTime;
    private const float FREQUENCY = 128f / 60f / 1f;
    public float freq;

    private Frequency()
    {

    }
    public static Frequency getInstance()
    {
        if (instance == null) instance = new Frequency();
        return instance;
    }

    public void startTimer( )
    {
        startTime = Time.time;
    }

    public void Update()
    {

        freq = Mathf.Cos(2 * Mathf.PI * FREQUENCY * (Time.time - startTime));

        //float moveSideway = Mathf.Sin(2 * Mathf.PI * FREQUENCY / 4f * (Time.time - startTime));

    }


}
