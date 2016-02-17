using UnityEngine;

public class BeatBarBehaviour : MonoBehaviour
{

	public float barSpeed;
	public Vector2 barSize;
	//private RectTransform rectTransform;
	private Vector2 left;
	private Transform myTransform;
	private Vector3 translation;

	void Start ()
	{
		//rectTransform = GetComponent<RectTransform>();
		myTransform = transform;
		left = -myTransform.right;
		translation = left*Time.fixedDeltaTime*barSpeed;
	}

	void FixedUpdate ()
	{
		//rt.sizeDelta = barSize;
		myTransform.Translate( translation );
		if (myTransform.position.x < -100.0f )
		{
			Destroy(this.gameObject);
		}
	}
}
