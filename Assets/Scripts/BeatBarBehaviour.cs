using UnityEngine;

public class BeatBarBehaviour : MonoBehaviour
{

	public float barSpeed;
	public Vector2 barSize;
	private RectTransform rectTransform;

	void Start ()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	void FixedUpdate ()
	{
		//rt.sizeDelta = barSize;
		rectTransform.transform.Translate( -transform.right * Time.fixedDeltaTime * barSpeed );
		if ( rectTransform.transform.position.x < 0.0 )
		{
			Destroy(this.gameObject);
		}
	}
}
