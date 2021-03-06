﻿using UnityEngine;

[System.Serializable]
public struct BirdBone
{
	public Transform bone;
	public Vector3 initialLocalPosition;
	public Vector3 initialWorldPosition;

	public Quaternion initialLocalRotation;
	//public Quaternion initialWorldRotation;

	public static BirdBone CreateBirdBone(Transform bone)
	{
		return new BirdBone
		{
			bone = bone,
			initialLocalPosition = bone.localPosition,
			initialWorldPosition = bone.position,
			initialLocalRotation = bone.localRotation,
			//initialWorldRotation = bone.rotation
		};
	}

}

