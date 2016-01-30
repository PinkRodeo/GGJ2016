using UnityEngine;
using System.Collections;

public class BeatAction 
{
	public Vector2 barSize;
	public Sprite beatSprite;
	private BeatBarBehaviour logic;

	public BeatAction()
	{
		logic = new BeatBarBehaviour();
	}
}
