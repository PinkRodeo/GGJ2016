using UnityEngine;
using System.Collections;

public class BirdWing
{

	private BirdBone shoulder;
	private BirdBone wing_1;
	private BirdBone wing_1_f;

	private BirdBone wing_2;
	private BirdBone wing_2_f;

	private BirdBone wing_3;
	private BirdBone wing_3_f;


	public BirdWing(Transform shoulderTransform, string suffix)
	{
		Debug.Log(shoulderTransform);
		shoulder = BirdBone.CreateBirdBone(shoulderTransform);

		wing_1 = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_1" + suffix));
		
		wing_1_f = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_F_1" + suffix));
		
		wing_2 = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_2" + suffix));
		wing_2_f = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_F_2" + suffix));

		wing_3 = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_3" + suffix));
		wing_3_f = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_F_3" + suffix));

		Log.Steb("Aww yess we loaded a birdwing with suffix " + suffix);
	}

	public void Update(float dt, float currentInput)
	{
		Log.Steb(currentInput);

	}

}
