using UnityEngine;
using System.Collections;
using System;

public class BeatBarBehaviour : MonoBehaviour
{

	public float barSpeed;
	public Vector2 barSize;

	void Start ()
	{

	}

	void Update ()
	{
		RectTransform rt = GetComponent<RectTransform>();
		//rt.sizeDelta = barSize;
		rt.transform.Translate( -transform.right * Time.deltaTime * barSpeed );
		if ( rt.transform.position.x < 0.0 )
		{
			Destroy(this.gameObject);
		}
	}
}
