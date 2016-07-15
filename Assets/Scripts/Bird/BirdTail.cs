using UnityEngine;

public class BirdTail
{
	private ControllerInput input;
	private BirdControl birdControl;

	private BirdBone tail_1;
	private BirdBone tail_2;
	private BirdBone tail_3;

	private float tail_held = 0f;



	public BirdTail(Transform firstTail, ControllerInput input, BirdControl birdControl)
	{
		this.input = input;
		this.birdControl = birdControl;

		tail_1 = BirdBone.CreateBirdBone(firstTail);
		tail_2 = BirdBone.CreateBirdBone(firstTail.FindInChildren("Tail_2"));
		tail_3 = BirdBone.CreateBirdBone(firstTail.FindInChildren("Tail_3"));

	}

	// Update is called once per frame
	public void Update (float dt) 
	{
		if (input.GetKey(ControllerAction.R1) || (birdControl.DEBUG_ENABLE_KEYBOARD && Input.GetKey(KeyCode.S)) )
		{
			tail_held += dt * 7f;
			tail_held = Mathf.Min(tail_held, 1f);
		}
		else
		{
			tail_held -= dt * 3f;
			tail_held = Mathf.Max(tail_held, 0f);
		}

		tail_1.bone.localRotation = tail_1.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(0, 30f, tail_held), Vector3.right);
		tail_2.bone.localRotation = tail_2.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(0, 50f, tail_held), Vector3.right);
		tail_3.bone.localRotation = tail_3.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(0, 20f, tail_held), Vector3.right);

	}
}
