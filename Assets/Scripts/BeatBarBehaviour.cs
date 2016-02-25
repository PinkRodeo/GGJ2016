using System;
using UnityEngine;
using UnityEngine.UI;

public class BeatBarBehaviour : MonoBehaviour
{
	public Vector2 barSize;
	//private RectTransform rectTransform;

	private RectTransform myTransform;
	
	public BeatGUIBar.Beat beat;
	public BeatGUIBar beatController;

	private Vector3 _originalLocalPos;


	private UnityEngine.UI.Image _spriteRenderer;

	private static readonly Vector3 FullBarScale = new Vector3(0.5f, 2f) * 1.5f;
	private static readonly Vector3 OtherBarScale = new Vector3(0.5f, 0.5f) * 1.5f;

	private const float spacing = 500f;

	private Color regularColor = new Color(1f, 1f, 1f, 0.2f);
	private Color hitColor = new Color(1f, 1f, 1f, 1f);

	public bool isPose = false;

	public void Initialize()
	{
		//rectTransform = GetComponent<RectTransform>();
		myTransform = GetComponent<RectTransform>();

		_originalLocalPos = myTransform.anchoredPosition3D;

		_originalLocalPos.y += 50f;


		if (beat.type == BeatGUIBar.BarType.Normal)
		{
			if (beat.count == 7)
			{
				myTransform.localScale = FullBarScale;
			}
			else
			{
				myTransform.localScale = OtherBarScale;
			}
		}
		else
		{
			regularColor = new Color(1f, 1f, 1f, 0.4f);

			myTransform.localScale = OtherBarScale;
		}
		if (isPose)
		{
			regularColor = new Color(1f, 1f, 1f, 0.6f);

			myTransform.localScale *= 1.2f;
		}

		_spriteRenderer = GetComponent<UnityEngine.UI.Image>();
		_spriteRenderer.color = regularColor;
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
	const float XoffsetFromLeft = 300f;

	private bool _mayUpdate = true;

	void LateUpdate()
	{
		if (!_mayUpdate)
			return;
		_originalLocalPos.x = (beat.time - beatController.globalTime) * spacing + XoffsetFromLeft;
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
			var gradient = BeatGUIBar.stebGlobal.timingGradient;

			LeanTween.cancel(gradient.gameObject);

			if (beat.count == 7)
			{
				gradient.color = new Color(1f, 1f, 1f, 1f);

				LeanTween.value(gradient.gameObject, 1f, 0.1f, 0.3f)
					.setOnUpdate(f =>
					{
						gradient.color = new Color(1f, 1f, 1f, f);
					})
					.setDelay(0.1f);
			}
			else
			{
				LeanTween.value(gradient.gameObject, 0.5f, 0.1f, 0.4f)
					.setOnUpdate(f =>
					{
						gradient.color = new Color(1f, 1f, 1f, f);
					});
			}


				//.setEase(LeanTweenType.easeOutQuint);

			if (isPose)
			{
				_originalLocalPos.x = XoffsetFromLeft;
				myTransform.anchoredPosition3D = _originalLocalPos;
				

				var tween = LeanTween.value(gameObject, 1f, 0f, 1f)
					.setOnUpdate(f =>
					{
						_spriteRenderer.color = new Color(1, 1, 1, f);
						myTransform.Translate(0, f, 0);
					})
					.setOnComplete(() => Destroy(this.gameObject));
				tween.delay = 0.3f;
				tween.setEase(LeanTweenType.easeOutCirc);

				_mayUpdate = false;
			}
			else if (beat.type == BeatGUIBar.BarType.Special)
			{
				_originalLocalPos.x = XoffsetFromLeft;
				myTransform.anchoredPosition3D = _originalLocalPos;


				var tween = LeanTween.value(gameObject, 1f, 0f, 1f)
					.setOnUpdate(f =>
					{
						_spriteRenderer.color = new Color(1, 1, 1, f);
						myTransform.Translate(0, -f, 0);
					})
					.setOnComplete(() => Destroy(this.gameObject));
				tween.delay = 0.3f;
				tween.setEase(LeanTweenType.easeOutCirc);

				_mayUpdate = false;
			}
			else
			{
				Destroy(this.gameObject);
	
			}
		}
	}

	
}
