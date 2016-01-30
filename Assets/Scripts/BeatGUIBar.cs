using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BeatGUIBar : MonoBehaviour 
{
	public float barSpeed;
	public float offset;
	public int BeatsPerMinute;
	private GameObject canvas;

	void Start () 
	{
		canvas = GameObject.Find("Canvas");
		Invoke( "AddBeatBar", 60.0f / BeatsPerMinute );
	}

	void Update () 
	{
		

	}
		
	private void AddBeatBar()
	{
		GameObject bar = new GameObject("beatBar", typeof(RectTransform));
		bar.AddComponent<CanvasRenderer>();
		bar.AddComponent<Image>();

		RectTransform rt = bar.GetComponent<RectTransform>();
		bar.transform.SetParent( canvas.transform );
		Vector3 p = rt.position;
		rt.transform.position = new Vector3( p.x + offset, p.y, p.z );

		BeatBarBehaviour behaviour = bar.AddComponent<BeatBarBehaviour>();
		behaviour.barSpeed = barSpeed;
		behaviour.barSize = new Vector2(10,50);

		Invoke( "AddBeatBar", 60.0f / BeatsPerMinute );
	}
}
