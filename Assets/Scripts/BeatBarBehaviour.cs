using UnityEngine;

public class BeatBarBehaviour : MonoBehaviour
{

	public float barSpeed;
	public Vector2 barSize;
	//private RectTransform rectTransform;
	private Vector2 left;
	private Transform myTransform;

	void Start ()
	{
		//rectTransform = GetComponent<RectTransform>();
		left = -transform.right;
		myTransform = transform;
	}

	void FixedUpdate ()
	{
		//rt.sizeDelta = barSize;
		myTransform.Translate( left * Time.fixedDeltaTime * barSpeed );
		if (myTransform.position.x < -100.0 )
		{
			Destroy(this.gameObject);
		}
	}
}
