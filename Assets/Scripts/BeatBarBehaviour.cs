using UnityEngine;

public class BeatBarBehaviour : MonoBehaviour
{
	public Vector2 barSize;
	//private RectTransform rectTransform;

	private RectTransform myTransform;
	
	public BeatGUIBar.Beat beat;
	public BeatGUIBar beatController;

	private Vector3 _originalLocalPos;


	private UnityEngine.UI.Image _spriteRenderer;

	private static readonly Vector3 FullBarScale = new Vector3(0.5f, 2f);
	private static readonly Vector3 OtherBarScale = new Vector3(0.5f, 0.5f);


	private static readonly Color regularColor = new Color(1f, 1f, 1f, 0.2f);
	private static readonly Color hitColor = new Color(1f, 1f, 1f, 1f);


	public void Initialize()
	{
		//rectTransform = GetComponent<RectTransform>();
		myTransform = GetComponent<RectTransform>();

		_originalLocalPos = myTransform.anchoredPosition3D;

		_spriteRenderer = GetComponent<UnityEngine.UI.Image>();
		_spriteRenderer.color = regularColor;


		if (beat.count == 7)
		{
			myTransform.localScale = FullBarScale;
		}
		else
		{
			myTransform.localScale = OtherBarScale;
		}
	}

	void FixedUpdate ()
	{
		/*
		myTransform.Translate( translation );
		if (myTransform.position.x < -100.0f )
		{
			Destroy(this.gameObject);
		}
		*/
	}

	const float OffsetToHighlight = 100f;
	const float XoffsetFromLeft = 100f;

	void LateUpdate()
	{
		_originalLocalPos.x = (beat.time - beatController.globalTime) * 300f + XoffsetFromLeft;
		//_originalLocalPos.x = XoffsetFromLeft;

		myTransform.anchoredPosition3D = _originalLocalPos;

		float xPos = _originalLocalPos.x - XoffsetFromLeft;

		if (xPos < OffsetToHighlight)
		{
			var highlight = xPos/OffsetToHighlight;
			_spriteRenderer.color = Color.Lerp(hitColor, regularColor, highlight);
		}

		if (_originalLocalPos.x < XoffsetFromLeft)
		{
			Destroy(this.gameObject);
		}
	}

	
}
