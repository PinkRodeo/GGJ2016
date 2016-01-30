using UnityEngine;
//using System.Collections;

public class BirdLeg
{
	private BirdBone hip;
	private BirdBone leg_upper;
	private BirdBone leg_lower;
	private BirdBone leg_heel;
	private BirdBone leg_feet;
	
	public BirdLeg(Transform hipTransform, string suffix)
	{
		hip = BirdBone.CreateBirdBone(hipTransform);

		leg_upper = BirdBone.CreateBirdBone(hipTransform.FindInChildren("Leg_Upper" + suffix));
		leg_lower = BirdBone.CreateBirdBone(hipTransform.FindInChildren("Leg_Lower" + suffix));
		leg_heel = BirdBone.CreateBirdBone(hipTransform.FindInChildren("Leg_Heel" + suffix));
		leg_feet = BirdBone.CreateBirdBone(hipTransform.FindInChildren("Leg_Feet" + suffix));
		
	}


}
